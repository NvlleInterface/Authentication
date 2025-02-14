using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TextswapAuthApi.Domaine.Configurations;

namespace TextswapAuthApi.Application.Common.Services;

public class RefreshTokenValidator
{
    private readonly TextswapAuthApiConfiguration _configuration;

    public RefreshTokenValidator(TextswapAuthApiConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool Validate(string refreshToken)
    {
        TokenValidationParameters validationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidAudience = _configuration.Audience,
            ValidIssuer = _configuration.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.RefreshTokenSecret))
        };

        JwtSecurityTokenHandler tokenHandle = new JwtSecurityTokenHandler();

        try
        {
            tokenHandle.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

