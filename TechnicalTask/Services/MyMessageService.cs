using TechnicalTask.Models;

namespace TechnicalTask.Services
{
    public class MyMessageService : IMyMessageService
    {
        private const string PATH = "./Logs/logs.txt";
        public async Task<string> LogMessage(MyMessageRequest log, ILogger<Program> logger)
        {
            try
            {
                if (!File.Exists(PATH))
                    File.Create(PATH);

                var logAsString = $"[Id] {log.Id}, [Date] {log.Date}, [Message] {log.TextContent}";
                await File.AppendAllTextAsync(PATH, Environment.NewLine + logAsString);

                return logAsString;

            }
            catch (IOException ex)
            {
                logger.LogError(ex, "An error occurred while writing to the log.");
                return "An error occurred while writing to the log.";
            }
        }
    }
}
