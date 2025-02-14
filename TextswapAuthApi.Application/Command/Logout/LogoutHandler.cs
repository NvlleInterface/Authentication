using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TextswapAuthApi.Application.Common.Services.RefreshTokenRepositories;
using TextswapAuthApi.Application.Wrappers;
using TextswapAuthApi.Domaine.Models;
using TextswapAuthApi.Domaine.Models.Authentication.Entities;
using TextswapAuthApi.Domaine.Models.Dto;

namespace TextswapAuthApi.Application.Command.Logout;

public sealed class LogoutHandler : IHandlerWrapper<LogoutCommand, TextswapAuthApiUserResponseDto>
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
    public async Task<Response<TextswapAuthApiUserResponseDto>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.RawUserId, out Guid userId))
        {
            return Response.Fail<TextswapAuthApiUserResponseDto>("Unauthorized", StatusCodes.Status401Unauthorized, "Unauthorized");
        }

        await _refreshTokenRepository.DeleteAllAsync(userId).ConfigureAwait(false);
        await _signInManager.SignOutAsync().ConfigureAwait(false);

        return Response.Ok<TextswapAuthApiUserResponseDto>(null!, string.Empty, StatusCodes.Status204NoContent, "NoContent");
    }
}
