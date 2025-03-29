using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Authentication.Presentation.OpenSpecifications.Constants;

namespace Authentication.Presentation.OpenSpecifications.Samples.Responses;

/// <summary>
/// Conflict sample.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ConflictResponseSample
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
    /// Conflict sample constructor.
    /// </summary>
    public ConflictResponseSample()
    {
        Status = 409;
        Type = RfcLinks.ConflictLink;
        Title = "Conflict";
        TraceId = Activity.Current?.Id;
    }
}
