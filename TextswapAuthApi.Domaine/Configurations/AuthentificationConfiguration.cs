using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextswapAuthApi.Domaine.Configurations;

public sealed class TextswapAuthApiConfiguration
{
    public string AccessTokenSecret { get; set; } = string.Empty;

    public int AccessTokenExpirattionMinutes { get; set; }

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string RefreshTokenSecret { get; set; } = string.Empty;

    public double RefreshTokenExpirattionMinutes { get; set; }
}   
