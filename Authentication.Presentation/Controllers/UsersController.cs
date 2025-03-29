using Authentication.Application.Accounts.CommandHandler;
using Authentication.Application.Accounts.QueriesHandler;
using Authentication.Presentation.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace Authentication.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IMediator _mediator { get; }

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/email")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _mediator.Send(new GetUserByEmailQuery(email), HttpContext.RequestAborted).ConfigureAwait(false);

            return ReturnActionResult.ActionResult(user);
        }

        [HttpGet("/name")]
        public async Task<IActionResult> GetUserByUserName(string username)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _mediator.Send(new GetAllUsersQuery(), HttpContext.RequestAborted).ConfigureAwait(false);

            return ReturnActionResult.ActionResult(users);
        }

        [HttpDelete("/email")]
        public async Task<IActionResult> DeleteUserByEMail(string email)
        {
            var user = await _mediator.Send(new DeleteUserByEmailCommand(email), HttpContext.RequestAborted).ConfigureAwait(false);

            return ReturnActionResult.ActionResult(user);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllUsers()
        {
            var user = await _mediator.Send(new DeleteAllUsersCommand(), HttpContext.RequestAborted).ConfigureAwait(false);

            return ReturnActionResult.ActionResult(user);
        }

        
    }
}
