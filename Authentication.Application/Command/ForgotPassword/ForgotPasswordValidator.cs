using FluentValidation;

namespace Authentication.Application.Command.ForgotPassword
{
    public sealed class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(o => o.Email).EmailAddress();
        }
    }
}
