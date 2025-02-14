using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using TextswapAuthApi.Api.OpenSpecifications.Constants;

namespace TextswapAuthApi.Api.OpenSpecifications.Samples.Responses;

[ExcludeFromCodeCoverage]
public sealed class ForbiddenResponseSample
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
    /// Errors.
    /// </summary>
    public ConcurrentDictionary<string, string[]> Errors { get; set; }

    /// <summary>
    /// Conflict sample constructor.
    /// </summary>
    public ForbiddenResponseSample()
    {
        Status = 403;
        Type = RfcLinks.ForbiddenLink;
        Title = "Forbidden";
        TraceId = Activity.Current?.Id;
        Errors = new ConcurrentDictionary<string, string[]>();
        Errors.TryAdd("Field", new []{ "Any error on specified field", "Any other error on specified field" });
    }
}
