using TextswapAuthApi.Domaine.Configurations;

namespace TextswapAuthApi.Application.Common.Services.TokenGenerators;

public sealed class RefreshTokenGenerator
{
    private readonly TextswapAuthApiConfiguration _configuration;
    private readonly TokenGenerator _tokenGenerator;

    public RefreshTokenGenerator(TextswapAuthApiConfiguration configuration, TokenGenerator tokenGenerator)
    {
        _configuration = configuration;
        _tokenGenerator = tokenGenerator;
    }
    public string GenerateRefreshToken()
    {

        return _tokenGenerator.GenerateToken(
        _configuration.RefreshTokenSecret,
        _configuration.Issuer,
        _configuration.Audience,
        _configuration.RefreshTokenExpirattionMinutes
        );
    }
}
