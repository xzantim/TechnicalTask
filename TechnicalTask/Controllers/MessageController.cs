using Microsoft.AspNetCore.Mvc;
using TechnicalTask.Models;
using TechnicalTask.Services;

namespace TechnicalTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController(IMyMessageService messageLogger, ILogger<Program> logger) : ControllerBase
    {
        private readonly IMyMessageService _messageLogger = messageLogger;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MyMessageRequest logModel) => Ok(await _messageLogger.LogMessage(logModel, logger));
    }
}
