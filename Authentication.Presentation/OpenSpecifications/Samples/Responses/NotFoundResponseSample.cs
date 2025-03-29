using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Authentication.Presentation.OpenSpecifications.Constants;

namespace Authentication.Presentation.OpenSpecifications.Samples.Responses;

/// <summary>
/// Not found response.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class NotFoundResponseSample
{
    /// <summary>
    /// Type.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Status.
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Trace unique identifier.
    /// </summary>
    public string? TraceId { get; set; }

    /// <summary>
    /// Not found response.
    /// </summary>
    public NotFoundResponseSample()
    {
        Status = 404;
        Type = RfcLinks.NotFoundLink;
        Title = "Not Found"; 
        TraceId = Activity.Current?.Id;
    }
}
