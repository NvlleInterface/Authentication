using Microsoft.AspNetCore.Mvc;

namespace TextswapAuthApi.Api.Response
{
    public static class ReturnActionResult
    {
        public static IActionResult ActionResult(dynamic refresh)
        {
            switch (refresh.StatusCode)
            {
                case StatusCodes.Status201Created:
                    return new CreatedResult("/login", refresh);
                case StatusCodes.Status204NoContent:
                    return new NoContentResult();
                case StatusCodes.Status400BadRequest:
                    return new BadRequestObjectResult(refresh);
                case StatusCodes.Status401Unauthorized:
                    return new UnauthorizedObjectResult(refresh);
                case StatusCodes.Status403Forbidden:
                    return new ForbidResult(refresh);
                case StatusCodes.Status404NotFound:
                    return new NotFoundObjectResult(refresh);
                case StatusCodes.Status409Conflict:
                    return new ConflictObjectResult(refresh);
                default:
                    return new OkObjectResult(refresh);
            }
        }

        //private IActionResult BadRequestModelState()
        //{
        //    IEnumerable<string> errorsMessage = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
        //    return BadRequest(new ErrorResponsesDto(errorsMessage));
        //}
    }
}
