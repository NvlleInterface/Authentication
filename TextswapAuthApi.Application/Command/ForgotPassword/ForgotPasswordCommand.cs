using TextswapAuthApi.Application.Wrappers;

namespace TextswapAuthApi.Application.Command.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email) : IRequestWrapper<string>
{
    private string? Link { get; set; }

    public void SetLink(string link) => Link = link;

    internal string GetLink()
    {
        return Link!;
    }
}

