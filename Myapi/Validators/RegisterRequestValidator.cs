using FluentValidation;
using Myapi.DTOs;

namespace Myapi.Validators
{
    public class RegisterRequestValidator:AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("User Name is required")
                .MaximumLength(100).WithMessage("User Name reached maximum of 100 characters");
            
            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("Password is required")
                .MaximumLength(100).WithMessage("Password reached maximum of 100 characters")
                .MinimumLength(6).WithMessage("Password must be at least 8 characters");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(100).WithMessage("Email reached maximum of 100 characters")
                .EmailAddress().WithMessage("A valid email is required");
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(100).WithMessage("First Name reached maximum of 100 characters");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(100).WithMessage("Last Name reached maximum of 100 characters");

        }
    }
}
