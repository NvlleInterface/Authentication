using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TextswapAuthApi.Application.Common.Services.MailMessage;
using TextswapAuthApi.Application.Wrappers;
using TextswapAuthApi.Domaine.Models;

namespace TextswapAuthApi.Application.Command.ForgotPassword;

public sealed class ForgotPasswordHandler : IHandlerWrapper<ForgotPasswordCommand, string>
{
    private readonly UserManager<IdentityUser> _userManager;
    public readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    public ForgotPasswordHandler(UserManager<IdentityUser> userManager,
        IMapper mapper,
        IEmailService emailService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<Response<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var link = request.GetLink();
        if (string.IsNullOrWhiteSpace(link))
        {
            return await GenerateLinkAsync(request.Email).ConfigureAwait(false);
        }

        return await SendEmailForRestPasswordAsync(request.Email, request.GetLink()).ConfigureAwait(false);


    }

    private async Task<Response<string>> GenerateLinkAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return Response.Fail<string>("Email is not regonize in the systeme", StatusCodes.Status404NotFound, "NotFound");
        }

        string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        return Response.Ok(resetToken, "Reset Token genereted", StatusCodes.Status200OK, "success");
    }

    private async Task<Response<string>> SendEmailForRestPasswordAsync(string email, string link)
    {
        var message = new Message(new string[] { email }, "Forgot password link", link);
        try
        {
            await _emailService.SendEmailAsync(message);

        }
        catch (Exception e)
        {
            return Response.Fail<string>(e.Message, StatusCodes.Status403Forbidden, "Exception");
        }

        return Response.Ok<string>(null!, $"Password changed request is sent on email {email}. Please open your email and click the link", StatusCodes.Status204NoContent, "NotContent");
    }
}