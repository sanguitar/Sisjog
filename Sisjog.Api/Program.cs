using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Serilog;
using Serilog.Events;
using Sisjog.Application.Interfaces;
using Sisjog.Application.Mapping;
using Sisjog.Application.Services;
using Sisjog.Infrastructure.Persistence;
using Sisjog.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

var seqServerUrl = builder.Configuration["Seq:ServerUrl"];

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "Sisjog.Api")
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
        .WriteTo.Console();

    if (!string.IsNullOrWhiteSpace(seqServerUrl))
    {
        loggerConfiguration.WriteTo.Seq(seqServerUrl);
    }
});

var azureAdSection = builder.Configuration.GetSection("AzureAd");
var azureAdConfigured =
    !string.IsNullOrWhiteSpace(azureAdSection["TenantId"]) &&
    !string.IsNullOrWhiteSpace(azureAdSection["ClientId"]);

var authentication = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
if (azureAdConfigured)
{
    authentication.AddMicrosoftIdentityWebApi(azureAdSection);
}
else
{
    authentication.AddJwtBearer();
}

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Configure ConnectionStrings:DefaultConnection usando User Secrets ou variável de ambiente.");
}

builder.Services.AddDbContext<SisjogDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddScoped<IVideoGameConsoleService, VideoGameConsoleService>();
builder.Services.AddScoped<IVideoGameConsoleRepository, VideoGameConsoleRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalReact", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.GetLevel = (httpContext, _, exception) =>
        exception is not null || httpContext.Response.StatusCode >= 500
            ? LogEventLevel.Error
            : httpContext.Response.StatusCode >= 400
                ? LogEventLevel.Warning
                : LogEventLevel.Information;
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowLocalReact");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", async (
    SisjogDbContext dbContext,
    ILoggerFactory loggerFactory,
    CancellationToken cancellationToken) =>
{
    var logger = loggerFactory.CreateLogger("HealthCheck");

    try
    {
        if (await dbContext.Database.CanConnectAsync(cancellationToken))
        {
            return Results.Text("Healthy", "text/plain");
        }

        logger.LogWarning("Database health check could not connect to SisjogDb.");
    }
    catch (Exception exception)
    {
        logger.LogError(exception, "Database health check failed for SisjogDb.");
    }

    return Results.Text("Unhealthy", "text/plain", statusCode: StatusCodes.Status503ServiceUnavailable);
})
.AllowAnonymous();

app.Run();
