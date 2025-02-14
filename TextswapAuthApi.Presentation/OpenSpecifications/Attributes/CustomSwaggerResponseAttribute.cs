using System.Diagnostics.CodeAnalysis;

namespace TextswapAuthApi.Api.OpenSpecifications.Attributes;

/// <summary>
/// Custom swagger response attribute.
/// </summary>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class CustomSwaggerResponseAttribute : Attribute
{
    /// <summary>
    /// Status code.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Response type.
    /// </summary>
    public Type ResponseType { get; set; }

    /// <summary>
    /// Media types.
    /// </summary>
    public string[]? MediaTypes { get; set; }

    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Example provider type.
    /// </summary>
    public Type? ExamplesProviderType { get; set; }

    /// <summary>
    /// Custom swagger response attribute constructor.
    /// </summary>
    /// <param name="statusCode">Status code.</param>
    /// <param name="responseType">Response type.</param>
    /// <param name="mediaTypes">Media types.</param>
    public CustomSwaggerResponseAttribute(int statusCode, Type responseType, params string[] mediaTypes)
    {
        _ = responseType ?? throw new ArgumentNullException(nameof(responseType));
        if (responseType != typeof(void) && (!mediaTypes?.Any() ?? true))
        {
            throw new ArgumentNullException(nameof(mediaTypes));
        }

        StatusCode = statusCode;
        ResponseType = responseType;
        MediaTypes = mediaTypes;
    }

    /// <summary>
    /// Custom swagger response attribute constructor.
    /// </summary>
    /// <param name="statusCode">Status code.</param>
    /// <param name="responseType">Response type.</param>
    /// <param name="exampleResponseType">Example response type.</param>
    /// <param name="mediaTypes">Media types.</param>
    public CustomSwaggerResponseAttribute(int statusCode, Type responseType, Type exampleResponseType, params string[] mediaTypes)
    {
        _ = responseType ?? throw new ArgumentNullException(nameof(responseType));
        if (responseType != typeof(void) && (!mediaTypes?.Any() ?? true))
        {
            throw new ArgumentNullException(nameof(mediaTypes));
        }

        StatusCode = statusCode;
        ResponseType = responseType;
        MediaTypes = mediaTypes;
        ExamplesProviderType = exampleResponseType;
    }
}
