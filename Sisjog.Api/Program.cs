using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Sisjog.Application.Interfaces;
using Sisjog.Application.Mapping;
using Sisjog.Application.Services;
using Sisjog.Infrastructure.Persistence;
using Sisjog.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

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

app.Run();
