using FluentValidation.AspNetCore;
using FluentValidation;
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

            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseMiddleware<ApiKeyMiddleware>();
            app.MapControllers();
            app.Run();
        }
    }
}
