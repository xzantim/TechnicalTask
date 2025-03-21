using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TechnicalTask.Models;

namespace TechnicalTask.Tests.IntegrationTests
{
    public class MessageTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;

        public MessageTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
        }

        [Fact]
        public async Task MessagePostValid_ReturnsOkObjectResult()
        {
            // Arrange
            var message = new MyMessageRequest(1, DateTime.UtcNow, "Test message 1");
            var expectedOutput = $"[Id] {message.Id}, [Date] {message.Date}, [Message] {message.TextContent}";
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("x-api-key", "3c395e34-60ef-433d-b681-b11acffb1f89");
            // Act
            var response = await _httpClient.PostAsync("/api/Message/", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseContent);
            Assert.Equal(responseContent, expectedOutput);
        }

        [Theory]
        [InlineData(0, "2023-02-01T12:23:34Z", "A test message", "Log Id must be greater than 0.")]
        [InlineData(1, null, "A test message", "Log Date must not be null.")]
        [InlineData(2, "2023-02-01T12:23:34Z", "", "Log Text Content cannot be empty.")]
        [InlineData(3, "2023-02-01T12:23:34Z", "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. " +
                    "Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis " +
                    "parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, " +
                    "pretium quis,.", "Log Text Content must be 255 characters or fewer.")]
        public async Task MessagePostInValidModel_ReturnsBadRequest(int id, string? date, string messageValue, string validationMessage)
        {
            // Arrange
            var message = new { Id = id, Date = date, TextContent = messageValue };
            var expectedOutput = $"[Id] {message.Id}, [Date] {message.Date}, [Message] {message.TextContent}";
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("x-api-key", "3c395e34-60ef-433d-b681-b11acffb1f89");
            // Act
            var response = await _httpClient.PostAsync("/api/Message/", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorMessages = JsonConvert.DeserializeObject<HttpValidationProblemDetails>(responseContent)?.Errors.SelectMany(x => x.Value).ToList() ?? [];
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(responseContent);
            Assert.Contains(validationMessage, errorMessages);
        }

        [Fact]
        public async Task EmptyApiKey_ReturnsUnAuthorized()
        {
            // Arrange
            var message = new MyMessageRequest(2, DateTime.UtcNow, "Test message 2");
            var expectedOutput = "Api Key was not provided.";
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("x-api-key", "");
            // Act
            var response = await _httpClient.PostAsync("/api/Message/", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.NotNull(responseContent);
            Assert.Equal(responseContent, expectedOutput);
        }

        [Fact]
        public async Task InvalidApiKey_ReturnsUnAuthorized()
        {
            // Arrange
            var message = new MyMessageRequest(1, DateTime.UtcNow, "Test message 3");
            var expectedOutput = "Unauthorized client.";
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("x-api-key", "asdf");
            // Act
            var response = await _httpClient.PostAsync("/api/Message/", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.NotNull(responseContent);
            Assert.Equal(responseContent, expectedOutput);
        }
    }
}
