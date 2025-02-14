using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TextswapAuthApi.Application.Common.Services.TokenGenerators;

public sealed class TokenGenerator
{
    public string GenerateToken(string secretKey, string issuer, string audience, double expirattionMinutes, IEnumerable<Claim>? claims = null)
    {
        SecurityKey secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer,
            audience,
            (IEnumerable<System.Security.Claims.Claim>)claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(expirattionMinutes),
            credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
