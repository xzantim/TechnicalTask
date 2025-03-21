using TechnicalTask.Models;

namespace TechnicalTask.Tests.UnitTests
{
    public class MyMessageRequestValidatorTests()
    {
        private readonly MyMessageRequestValidator _validator = new();

        [Fact]
        public void MessageRequestIdMustBeGreaterThanOne()
        {
            // Arrange
            var messageRequest = new MyMessageRequest(0, DateTime.UtcNow, "message text");
            // Act
            var result = _validator.Validate(messageRequest);
            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Log Id must be greater than 0.", result.Errors.Select(x => x.ErrorMessage));
        }

        [Fact]
        public void MessageRequestDateCannotBeNull()
        {
            // Arrange
            var messageRequest = new MyMessageRequest(1, null, "message text");
            // Act
            var result = _validator.Validate(messageRequest);
            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Log Date must not be null.", result.Errors.Select(x => x.ErrorMessage));
        }

        [Fact]
        public void MessageRequestTextContentCannotBeEmpty()
        {
            // Arrange
            var messageRequest = new MyMessageRequest(1, DateTime.UtcNow, "");
            // Act
            var result = _validator.Validate(messageRequest);
            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Log Text Content cannot be empty.", result.Errors.Select(x => x.ErrorMessage));
        }

        [Fact]
        public void MessageRequestTextContentMustBe255OrFewerCharacters()
        {
            // Arrange
            var messageRequest = new MyMessageRequest(1, DateTime.UtcNow, "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis,.");
            // Act
            var result = _validator.Validate(messageRequest);
            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Log Text Content must be 255 characters or fewer.", result.Errors.Select(x => x.ErrorMessage));
        }
    }
}
