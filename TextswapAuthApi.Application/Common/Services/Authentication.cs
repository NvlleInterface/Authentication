using Microsoft.AspNetCore.Identity;
using TextswapAuthApi.Application.Common.Services.RefreshTokenRepositories;
using TextswapAuthApi.Application.Common.Services.TokenGenerators;
using TextswapAuthApi.Domaine.Models.Authentication.Entities;
using TextswapAuthApi.Domaine.Models.Dto;

namespace TextswapAuthApi.Application.Common.Services;

public class Authentication
{
    private readonly AccessTokenGenerator _accessTokenGenerator;
    private readonly RefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenRepository<RefreshToken> _refreshTokenRepository;
    public Authentication(
        AccessTokenGenerator accessTokenGenerator,
        RefreshTokenGenerator refreshTokenGenerator,
        IRefreshTokenRepository<RefreshToken> refreshTokenRepository)
    {
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
    }
    public async Task<TextswapAuthApiUserResponseDto> Authenticate(IdentityUser user, string roleName)
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

        return (new TextswapAuthApiUserResponseDto()
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            Role = roleName
        });
    }
}
