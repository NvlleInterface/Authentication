using TextswapAuthApi.Application.Wrappers;

namespace TextswapAuthApi.Application.Command.ResetPassword;

public sealed record ResetPasswordCommand(string Password, string ConfirmPassword, string Email, string Token) : IRequestWrapper<string>
{
   
}

