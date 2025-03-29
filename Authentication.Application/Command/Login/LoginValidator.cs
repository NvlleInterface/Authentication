

using FluentValidation;

namespace Authentication.Application.Command.Login
{
    public sealed class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(o => o.Email).EmailAddress();
            RuleFor(o => o.Password).NotEmpty().NotNull();
        }
    }
}
