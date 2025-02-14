namespace TextswapAuthApi.Application.Common.Services.RefreshTokenRepositories;

public interface IRefreshTokenRepository<T> where T : class
{
    Task<T> GetByTokenAsync(string token);

    Task CreateAsync(T identity);

    Task DeleteAsync(Guid id);

    Task DeleteAllAsync(Guid userId);
}

