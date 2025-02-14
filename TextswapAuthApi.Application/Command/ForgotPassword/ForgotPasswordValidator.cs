using FluentValidation;

namespace TextswapAuthApi.Application.Command.ForgotPassword
{
    public sealed class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(o => o.Email).EmailAddress();
        }
    }
}
