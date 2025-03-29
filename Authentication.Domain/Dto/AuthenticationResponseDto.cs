namespace Authentication.Domain.Dto;

public class AuthenticationResponseDto
{
    public string? Token { get; set; }

    public string? RefreshToken { get; set; }

    public string? Role { get; set; }
}
