using TechnicalTask.Models;

namespace TechnicalTask.Services
{
    public interface IMyMessageService
    {
        public Task<string> LogMessage(MyMessageRequest log, ILogger<Program> logger);
    }
}
