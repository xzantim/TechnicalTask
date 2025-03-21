using FluentValidation;

namespace TechnicalTask.Models
{
    public class MyMessageRequestValidator : AbstractValidator<MyMessageRequest>
    {
        public MyMessageRequestValidator()
        {
            RuleFor(l => l.Id)
                .GreaterThan(0)
                .WithMessage("Log Id must be greater than 0.");

            RuleFor(l => l.Date)
                .NotNull()
                .WithMessage("Log Date must not be null.");

            RuleFor(l => l.TextContent)
                .NotEmpty()
                .WithMessage("Log Text Content cannot be empty.")
                .MaximumLength(255)
                .WithMessage("Log Text Content must be 255 characters or fewer.");
        }
    }
}
