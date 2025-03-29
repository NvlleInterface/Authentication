using System.Diagnostics.CodeAnalysis;

namespace Authentication.Presentation.OpenSpecifications.Constants;

[ExcludeFromCodeCoverage]
public static class MediaTypes
{
    public const string Request = "application/json";
    public const string Response = "application/json";
    public const string ResponseProblem = "application/problem+json";
}
