using Authentication.Application.Wrappers;
using Authentication.Domain.Dto;

namespace Authentication.Application.Command.Register;

public record RegisterCommand(string UserName, string Email, string Password, string ConfirmPassword) : IRequestWrapper<ErrorResponsesDto>
{
    private string? ConfirmationUrl { get; set; }

    public void SetConfirmationUrl(string? confirmationurls) => ConfirmationUrl = confirmationurls;

    public bool IsConfirmPassword() => Password == ConfirmPassword;
};