using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TextswapAuthApi.Api.OpenSpecifications.Attributes;
using TextswapAuthApi.Api.OpenSpecifications.Constants;
using TextswapAuthApi.Api.OpenSpecifications.Samples.Responses;
using TextswapAuthApi.Api.Response;
using TextswapAuthApi.Application.Command.ForgotPassword;
using TextswapAuthApi.Application.Command.Logout;
using TextswapAuthApi.Application.Command.Refresh;
using TextswapAuthApi.Application.Command.Register;
using TextswapAuthApi.Application.Command.ResetPassword;

namespace TextswapAuthApi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthentificationController : ControllerBase
{
    private IMediator _mediator { get; }

    public AuthentificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [Consumes(MediaTypes.Request)]
    [CustomSwaggerResponse(StatusCodes.Status204NoContent, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status400BadRequest, typeof(ValidationProblemDetails), typeof(BadRequestResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status401Unauthorized, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status403Forbidden, typeof(ProblemDetails), typeof(ForbiddenResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status404NotFound, typeof(ProblemDetails), typeof(NotFoundResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status415UnsupportedMediaType, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status500InternalServerError, typeof(void))]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var confirmationUrl = Request.Headers["confirmationUrl"];
        command.SetConfirmationUrl(confirmationUrl);

        var register = await _mediator.Send(command, HttpContext.RequestAborted).ConfigureAwait(false);

        return ReturnActionResult.ActionResult(register);
    }

    [Authorize]
    [HttpPost("refresh")]
    [Consumes(MediaTypes.Request)]
    [CustomSwaggerResponse(StatusCodes.Status204NoContent, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status400BadRequest, typeof(ValidationProblemDetails), typeof(BadRequestResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status401Unauthorized, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status403Forbidden, typeof(ProblemDetails), typeof(ForbiddenResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status404NotFound, typeof(ProblemDetails), typeof(NotFoundResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status415UnsupportedMediaType, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status500InternalServerError, typeof(void))]
    public async Task<IActionResult> Refresh([FromBody] RefreshCommand command)
    {
        var refresh = await _mediator.Send(command, HttpContext.RequestAborted).ConfigureAwait(false);

        return ReturnActionResult.ActionResult(refresh);
    }

    [HttpPost("login")]
    [Consumes(MediaTypes.Request)]
    [CustomSwaggerResponse(StatusCodes.Status204NoContent, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status400BadRequest, typeof(ValidationProblemDetails), typeof(BadRequestResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status401Unauthorized, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status403Forbidden, typeof(ProblemDetails), typeof(ForbiddenResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status404NotFound, typeof(ProblemDetails), typeof(NotFoundResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status415UnsupportedMediaType, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status500InternalServerError, typeof(void))]
    public async Task<IActionResult> Login(ForgotPasswordCommand command)
    {
        var login = await _mediator.Send(command, HttpContext.RequestAborted).ConfigureAwait(false);

        return ReturnActionResult.ActionResult(login);
    }

    [Authorize]
    [HttpPost("logout")]
    [Consumes(MediaTypes.Request)]
    [CustomSwaggerResponse(StatusCodes.Status204NoContent, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status400BadRequest, typeof(ValidationProblemDetails), typeof(BadRequestResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status401Unauthorized, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status403Forbidden, typeof(ProblemDetails), typeof(ForbiddenResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status404NotFound, typeof(ProblemDetails), typeof(NotFoundResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status415UnsupportedMediaType, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status500InternalServerError, typeof(void))]
    public async Task<IActionResult> Logout()
    {
        var rawuserId = HttpContext.User.FindFirstValue("id");
        var command = new LogoutCommand(rawuserId);

        var logout = await _mediator.Send(command, HttpContext.RequestAborted).ConfigureAwait(false);

        return ReturnActionResult.ActionResult(logout);
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    [Consumes(MediaTypes.Request)]
    [CustomSwaggerResponse(StatusCodes.Status204NoContent, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status400BadRequest, typeof(ValidationProblemDetails), typeof(BadRequestResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status401Unauthorized, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status403Forbidden, typeof(ProblemDetails), typeof(ForbiddenResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status404NotFound, typeof(ProblemDetails), typeof(NotFoundResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status415UnsupportedMediaType, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status500InternalServerError, typeof(void))]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
    {
        var result = await _mediator.Send(command, HttpContext.RequestAborted).ConfigureAwait(false);
        if (result.Error)
        {
            return ReturnActionResult.ActionResult(result);
        }

        var link = Url.Action(nameof(ResetPassword), "Authentification", new { result.Data, email = command.Email }, Request.Scheme);
        command.SetLink(link!);

        var response = await _mediator.Send(command, HttpContext.RequestAborted).ConfigureAwait(false);

        return ReturnActionResult.ActionResult(response);
    }

    //[Authorize] 
    [HttpPost("reset-password")]
    [Consumes(MediaTypes.Request)]
    [CustomSwaggerResponse(StatusCodes.Status204NoContent, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status400BadRequest, typeof(ValidationProblemDetails), typeof(BadRequestResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status401Unauthorized, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status403Forbidden, typeof(ProblemDetails), typeof(ForbiddenResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status404NotFound, typeof(ProblemDetails), typeof(NotFoundResponseSample), MediaTypes.ResponseProblem)]
    [CustomSwaggerResponse(StatusCodes.Status415UnsupportedMediaType, typeof(void))]
    [CustomSwaggerResponse(StatusCodes.Status500InternalServerError, typeof(void))]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        var resetPassword = await _mediator.Send(command, HttpContext.RequestAborted).ConfigureAwait(false);

        return ReturnActionResult.ActionResult(resetPassword);
    }
}
