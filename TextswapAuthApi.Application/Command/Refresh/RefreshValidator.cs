using FluentValidation;

namespace TextswapAuthApi.Application.Command.Refresh
{
    public sealed class RefreshValidator : AbstractValidator<RefreshCommand>
    {
        public RefreshValidator()
        {
            RuleFor(o => o.RefreshToken).NotEmpty().NotNull();
        }
    }
}
