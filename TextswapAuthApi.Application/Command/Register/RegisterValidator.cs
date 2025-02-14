using FluentValidation;

namespace TextswapAuthApi.Application.Command.Register
{
    public sealed class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            RuleFor(o => o.Email).EmailAddress();
            RuleFor(o => o.UserName).NotEmpty().NotNull();
            RuleFor(o => o.Password).NotEmpty().NotNull();
            RuleFor(c => c.ConfirmPassword).Equal(o => o.Password);
        }
    }
}
