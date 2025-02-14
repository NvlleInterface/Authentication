namespace TextswapAuthApi.Domaine.Models.Dto;

public sealed class TextswapAuthApiUserResponseDto
{
    public string? Token { get; set; }

    public string? RefreshToken { get; set; }

    public string? Role { get; set; }
}
