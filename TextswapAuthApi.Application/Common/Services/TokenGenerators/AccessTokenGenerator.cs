using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TextswapAuthApi.Domaine.Configurations;

namespace TextswapAuthApi.Application.Common.Services.TokenGenerators;

public class AccessTokenGenerator
{
    private readonly TextswapAuthApiConfiguration _configuration;
    private readonly TokenGenerator _tokenGenerator;

    public AccessTokenGenerator(TextswapAuthApiConfiguration configuration, TokenGenerator tokenGenerator)
    {
        _configuration = configuration;
        _tokenGenerator = tokenGenerator;
    }

    public string GenerateAccessToken(IdentityUser user, string roleName)
    {
        List<Claim> claims = new List<Claim>()
        {
            new("userid", user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Role, roleName),
            new Claim("Name",user.UserName!),
            new Claim("role",roleName)
        };

        return _tokenGenerator.GenerateToken(
            _configuration.AccessTokenSecret,
            _configuration.Issuer,
            _configuration.Audience,
            _configuration.AccessTokenExpirattionMinutes,
            claims);
    }
}
