using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Text;
using TextswapAuthApi.Application.Wrappers;
using TextswapAuthApi.Domaine.Models;

namespace TextswapAuthApi.Application.Command.ResetPassword;

public sealed class ResetPasswordHandler : IHandlerWrapper<ResetPasswordCommand, string>
{
    private readonly UserManager<IdentityUser> _userManager;
    public ResetPasswordHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Response.Fail<string>("UserName or Password is not found", StatusCodes.Status404NotFound, "NotFound");
        }

        var resetPassResult = await _userManager.ResetPasswordAsync(user, request.Token, request.Password).ConfigureAwait(false);
        if(resetPassResult.Errors.Any())
        {
            StringBuilder error = new StringBuilder();
            foreach (var item in resetPassResult.Errors)
            {
                error.AppendLine($"{item.Code} : {item.Description}");
            }
            return Response.Fail<string>(error.ToString(), StatusCodes.Status400BadRequest, "BadRequest");
        }
        return Response.Ok<string>(null!, "Password rested", StatusCodes.Status204NoContent, "success");
    }

}