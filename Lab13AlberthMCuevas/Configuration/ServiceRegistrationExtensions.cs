using System.Text;
using Lab13AlberthMCuevas.Application.UseCase.Reportes.Handlers;
using Lab13AlberthMCuevas.Domain.Ports;
using Lab13AlberthMCuevas.Infrastructure.Adapters;
using Lab13AlberthMCuevas.Infrastructure.Data.Context;
using Lab13AlberthMCuevas.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Lab13AlberthMCuevas.Configuration;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurar DbContext con MySQL
        services.AddDbContext<LinqContext>(options =>
            options.UseMySql(
                configuration.GetConnectionString("DefaultConnection") ?? "Server=localhost;Port=3306;Database=linqexample;User=root;Password=",
                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection") ?? "Server=localhost;Port=3306;Database=linqexample;User=root;Password=")
            ));

        // Registrar UnitOfWork y Repositorios
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Registrar ExcelService
        services.AddScoped<IExcelService, ExcelService>();

        // Registrar MediatR - Busca handlers en el proyecto Application
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetClientsReportHandler).Assembly));

        services.AddEndpointsApiExplorer(); 
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Lab13 Reports API",
                Version = "v1",
                Description = "API de Reportes con Arquitectura Limpia - Generación de Excel con ClosedXML"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Ejemplo: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
    
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])
                    )
                };
            });
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Administrador", p => p.RequireRole("Administrador"));
            options.AddPolicy("Cliente", p => p.RequireRole("Cliente"));
            options.AddPolicy("Freelancer", p => p.RequireRole("Freelancer"));
        });



        return services;
    }
    
}