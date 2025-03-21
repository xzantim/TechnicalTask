using Microsoft.Extensions.Logging;
using Moq;
using TechnicalTask.Models;
using TechnicalTask.Services;

namespace TechnicalTask.Tests.UnitTests
{
    public class MyMessageServiceTests
    {
        private readonly MyMessageService _service;
        private readonly Mock<ILogger<Program>> _mockLogger;

        public MyMessageServiceTests()
        {
            _service = new MyMessageService();
            _mockLogger = new Mock<ILogger<Program>>();
        }

        [Fact]
        public async Task ServiceWritesToFileAndReturnsLogAsString_Success()
        {
            // Arrange
            var request = new MyMessageRequest(1, DateTime.UtcNow, "Test message");
            var expectedOutput = $"[Id] {request.Id}, [Date] {request.Date}, [Message] {request.TextContent}";
            // act
            var response = await _service.LogMessage(request, _mockLogger.Object);
            // Assert
            Assert.IsType<string>(response);
            Assert.Equal(expectedOutput, response);
        }
    }
}
