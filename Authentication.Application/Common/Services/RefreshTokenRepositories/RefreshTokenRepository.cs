
using Authentication.Domain.Models;
using Authentication.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Application.Common.Services.RefreshTokenRepositories;

internal class RefreshTokenRepository : IRefreshTokenRepository<RefreshToken>
{
    private readonly AuthenticationContext _context;

    public RefreshTokenRepository(AuthenticationContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllAsync(Guid userId)
    {
        IEnumerable<RefreshToken> refreshTokens = await _context.RefreshTokens.Where(t => t.UserId == userId).ToListAsync();

        _context.RefreshTokens.RemoveRange(refreshTokens);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var refreshToken = await _context.RefreshTokens.FindAsync(id);
        if (refreshToken != null)
        {
            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens.FirstAsync(u => u.Token.Equals(token));
    }
}

