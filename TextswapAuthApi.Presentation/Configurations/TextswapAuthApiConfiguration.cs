namespace TextswapAuthApi.Presentation.Configurations
{
    public sealed class TextswapAuthApiConfiguration
    {
        public string AccessTokenSecret { get; set; } = string.Empty;

        public int AccessTokenExpirattionMinutes { get; set; }

        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public string RefreshTokenSecret { get; set; } = string.Empty;

        public double RefreshTokenExpirattionMinutes { get; set; }
    }

}
