using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Sisjog.Application.Interfaces;
using Sisjog.Application.Mapping;
using Sisjog.Application.Services;
using Sisjog.Infrastructure.Persistence;
using Sisjog.Application.Interfaces;
using Sisjog.Infrastructure.Services;
using VideoGameConsoleService = Sisjog.Infrastructure.Services.VideoGameConsoleService;
using Sisjog.Infrastructure.Repository;






var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SisjogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddScoped<IVideoGameConsoleService, VideoGameConsoleService>();

builder.Services.AddScoped<IVideoGameConsoleService, VideoGameConsoleService>();
builder.Services.AddScoped<IVideoGameConsoleRepository, VideoGameConsoleRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalReact", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // React rodando aqui
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseHttpsRedirection();
app.UseCors("AllowLocalReact");


app.UseAuthorization();

app.MapControllers();

app.Run();
