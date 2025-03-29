using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Authentication.Application.Common.Services.RefreshTokenRepositories;
using Authentication.Application.Wrappers;
using Authentication.Domain;
using Authentication.Domain.Dto;
using Authentication.Domain.Models;

namespace Authentication.Application.Command.Logout;

public sealed class LogoutHandler : IHandlerWrapper<LogoutCommand, AuthenticationResponseDto>
{
    private readonly IRefreshTokenRepository<RefreshToken> _refreshTokenRepository;
    private readonly SignInManager<IdentityUser> _signInManager;
    public LogoutHandler(
       IRefreshTokenRepository<RefreshToken> refreshTokenRepository,
       SignInManager<IdentityUser> signInManager
       )
    {
        _refreshTokenRepository = refreshTokenRepository;
        _signInManager = signInManager;
    }
    public async Task<Response<AuthenticationResponseDto>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.RawUserId, out Guid userId))
        {
            return Response.Fail<AuthenticationResponseDto>("Unauthorized", StatusCodes.Status401Unauthorized, "Unauthorized");
        }

        await _refreshTokenRepository.DeleteAllAsync(userId).ConfigureAwait(false);
        await _signInManager.SignOutAsync().ConfigureAwait(false);

        return Response.Ok<AuthenticationResponseDto>(null!, string.Empty, StatusCodes.Status204NoContent, "NoContent");
    }
}
