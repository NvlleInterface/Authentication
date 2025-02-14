﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TextswapAuthApi.Application.Common.Services;
using TextswapAuthApi.Application.Wrappers;
using TextswapAuthApi.Domaine.Models;
using TextswapAuthApi.Domaine.Models.Dto;

namespace TextswapAuthApi.Application.Command.Login;

public sealed class LoginHandler : IHandlerWrapper<LoginCommand, TextswapAuthApiUserResponseDto>
{
    private readonly UserManager<IdentityUser> _userManager;
    public readonly IMapper _mapper;
    private readonly Authentication _authenticator;
    private readonly SignInManager<IdentityUser> _signInManager;
    public LoginHandler(UserManager<IdentityUser> userManager,
        Authentication authenticator,
        IMapper mapper,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _authenticator = authenticator;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    public async Task<Response<TextswapAuthApiUserResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // logic to get the user role
        // get the user object based on Email
        // IdentityUser user = new IdentityUser(inputModel.UserName);
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Response.Fail<TextswapAuthApiUserResponseDto>("UserName or Password is not found", StatusCodes.Status404NotFound, "NotFound");
        }

        bool isCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password).ConfigureAwait(false);
        if (!isCorrectPassword)
        {
            return Response.Fail<TextswapAuthApiUserResponseDto>("Your password isn't correct", StatusCodes.Status401Unauthorized, "Unauthorized");
        }

        var role = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        if (role.Count == 0)
        {
            await _signInManager.SignOutAsync();
            return Response.Fail<TextswapAuthApiUserResponseDto>("User is not activated with role. Please contact admin on mahesh@myapp.com", StatusCodes.Status401Unauthorized, "Unauthorized");
        }

        //read the rolename
        var roleName = role[0];

        var response = await _authenticator.Authenticate(user, roleName).ConfigureAwait(false);

        //return response;
        return Response.Ok(response, "Login success", StatusCodes.Status200OK, "success");

    }
}