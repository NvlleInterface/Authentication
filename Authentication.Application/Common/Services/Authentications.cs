using Microsoft.AspNetCore.Identity;
using Authentication.Application.Common.Services.RefreshTokenRepositories;
using Authentication.Application.Common.Services.TokenGenerators;
using Authentication.Domain.Models;
using Authentication.Domain.Dto;

namespace Authentication.Application.Common.Services;

public class Authentications
{
    private readonly AccessTokenGenerator _accessTokenGenerator;
    private readonly RefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenRepository<RefreshToken> _refreshTokenRepository;
    public Authentications(
        AccessTokenGenerator accessTokenGenerator,
        RefreshTokenGenerator refreshTokenGenerator,
        IRefreshTokenRepository<RefreshToken> refreshTokenRepository)
    {
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
    }
    public async Task<AuthenticationResponseDto> Authenticate(IdentityUser user, string roleName)
    {
        string accessToken = _accessTokenGenerator.GenerateAccessToken(user, roleName);
        string refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

        RefreshToken refreshTokenDto = new()
        {
            Token = refreshToken,
            UserId = Guid.Parse(user.Id),
            Role = roleName,
        };
        await _refreshTokenRepository.CreateAsync(refreshTokenDto);

        return (new AuthenticationResponseDto()
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            Role = roleName
        });
    }
}
