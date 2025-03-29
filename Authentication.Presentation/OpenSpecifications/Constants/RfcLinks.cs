
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Authentication.Presentation.OpenSpecifications.Constants
{
    [ExcludeFromCodeCoverage]
    public static class RfcLinks
    {
        public static readonly string BadRequestLink = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        public static readonly string ForbiddenLink = "https://tools.ietf.org/html/rfc7231#section-6.5.3";
        public static readonly string ConflictLink = "https://tools.ietf.org/html/rfc7231#section-6.5.8";
        public static readonly string UnsupportedMediaType = "https://tools.ietf.org/html/rfc7231#section-6.5.13";
        public static readonly string NotFoundLink = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
        public static readonly string UnAuthorizedLink = "https://tools.ietf.org/html/rfc7235#section-3.1";
        public static readonly string InternalServerError = "https://tools.ietf.org/html/rfc7231#section-6.6.1";


        public static ProblemDetails GetConflict()
        {
            return new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Type = ConflictLink,
                Title = "Conflict"
            };
        }
        
        public static ProblemDetails GetNotFound()
        {
            return new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Type = NotFoundLink,
                Title = "Not Found"
            };
        }
    }
}
