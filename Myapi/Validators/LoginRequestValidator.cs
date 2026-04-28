using FluentValidation;
using Myapi.DTOs;

namespace Myapi.Validators
{
    public class LoginRequestValidator: AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {

        RuleFor(x => x.Username)
                .NotEmpty().WithMessage("User Name is required")
                .MaximumLength(100).WithMessage("User Name reached maximum of 100 characters");

        RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MaximumLength(100).WithMessage("Password reached maximum of 100 characters")
                .MinimumLength(6).WithMessage("Password must be at least 8 characters");
        }

    }
}
