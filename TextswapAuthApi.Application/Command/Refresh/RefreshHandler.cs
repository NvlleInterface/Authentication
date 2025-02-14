

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TextswapAuthApi.Application.Common.Services;
using TextswapAuthApi.Application.Common.Services.RefreshTokenRepositories;
using TextswapAuthApi.Application.Wrappers;
using TextswapAuthApi.Domaine.Models;
using TextswapAuthApi.Domaine.Models.Authentication.Entities;
using TextswapAuthApi.Domaine.Models.Dto;

namespace TextswapAuthApi.Application.Command.Refresh;

public sealed class RefreshHandler : IHandlerWrapper<RefreshCommand, TextswapAuthApiUserResponseDto>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IRefreshTokenRepository<RefreshToken> _refreshTokenRepository;
    private readonly RefreshTokenValidator _refreshTokenValidator;
    private readonly Authentication _authenticator;
    public RefreshHandler(
        UserManager<IdentityUser> userManager,
        RefreshTokenValidator refreshTokenValidator,
        IRefreshTokenRepository<RefreshToken> refreshTokenRepository,
        Authentication authenticator
        )
    {
        _userManager = userManager;
        _refreshTokenValidator = refreshTokenValidator;
        _refreshTokenRepository = refreshTokenRepository;
        _authenticator = authenticator;
    }
    public async Task<Response<TextswapAuthApiUserResponseDto>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        bool isvalidRefreshToken = _refreshTokenValidator.Validate(request.RefreshToken);
        if (!isvalidRefreshToken)
        {
            return Response.Fail<TextswapAuthApiUserResponseDto>("Invalid refresh token.", StatusCodes.Status401Unauthorized, "Invalid");
        }

        var refreshTokenDTO = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken).ConfigureAwait(false);
        if (refreshTokenDTO == null)
        {
            return Response.Fail<TextswapAuthApiUserResponseDto>("Invalid refresh token.", StatusCodes.Status401Unauthorized, "Invalid");
        }

        await _refreshTokenRepository.DeleteAsync(refreshTokenDTO.Id).ConfigureAwait(false);

        var user = await _userManager.FindByIdAsync(refreshTokenDTO.UserId.ToString());
        if (user == null)
        {
            return Response.Fail<TextswapAuthApiUserResponseDto>("Not User found.", StatusCodes.Status404NotFound, "NotFound");
        }

        var role = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        if (role.Count == 0)
        {
            // await _signInManager.SignOutAsync();
            return Response.Fail<TextswapAuthApiUserResponseDto>("User is not activated with role. Please contact admin on mahesh@myapp.com", StatusCodes.Status401Unauthorized, "Unauthorized");
        }

        //read the rolename
        var roleName = role[0];
        var response = await _authenticator.Authenticate(user, roleName).ConfigureAwait(false);

        return Response.Ok(response, "Refresh success", StatusCodes.Status200OK, "success");
    }
}
