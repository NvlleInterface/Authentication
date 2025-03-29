using Authentication.Domain.Configurations;

namespace Authentication.Application.Common.Services.TokenGenerators;

public sealed class RefreshTokenGenerator
{
    private readonly AuthenticationConfiguration _configuration;
    private readonly TokenGenerator _tokenGenerator;

    public RefreshTokenGenerator(AuthenticationConfiguration configuration, TokenGenerator tokenGenerator)
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
