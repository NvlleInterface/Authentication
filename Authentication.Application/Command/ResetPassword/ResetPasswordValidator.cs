using FluentValidation;

namespace Authentication.Application.Command.ResetPassword
{
    public sealed class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(o => o.Email).EmailAddress();
            RuleFor(o => o.Token).NotEmpty().NotNull();
            RuleFor(o => o.Password).NotEmpty().NotNull();
            RuleFor(c => c.ConfirmPassword).Equal(o => o.Password);
        }
    }
}
