using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using TechnicalTask.Services;

namespace TechnicalTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders(); // Clear default providers
            builder.Logging.AddConsole(); // Add Console logging provider
            builder.Logging.AddDebug(); // Add Debug logging provider
            builder.Services.AddEndpointsApiExplorer();
            // Added to allow testing via swagger using the api key to authenticate
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
                {
                    Name = "x-api-key",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Authorization by x-api-key inside request's header",
                    Scheme = "ApiKeyScheme"
                });
                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };
                var requirement = new OpenApiSecurityRequirement
                {
                   { key, new List<string>() }
                };
                c.AddSecurityRequirement(requirement);
            });
            // Add services to the container.
            builder.Services.AddScoped<IMyMessageService, MyMessageService>();
            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddProblemDetails();
            builder.Services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
            var app = builder.Build();// Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseMiddleware<ApiKeyMiddleware>();
            app.MapControllers();
            app.Run();
        }
    }
}
