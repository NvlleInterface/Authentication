using TextswapAuthApi.Application.Wrappers;
using TextswapAuthApi.Domaine.Models.Dto;

namespace TextswapAuthApi.Application.Command.Register;

public record RegisterCommand(string UserName, string Email, string Password, string ConfirmPassword) : IRequestWrapper<ErrorResponsesDto>
{
    private string? ConfirmationUrl { get; set; }

    public void SetConfirmationUrl(string? confirmationurls) => ConfirmationUrl = confirmationurls;

    public bool IsConfirmPassword() => Password == ConfirmPassword;
};