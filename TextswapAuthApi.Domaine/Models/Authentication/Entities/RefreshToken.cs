
namespace TextswapAuthApi.Domaine.Models.Authentication.Entities
{
    public sealed class RefreshToken
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Token { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
