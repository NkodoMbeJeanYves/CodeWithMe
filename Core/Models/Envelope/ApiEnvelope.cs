using System.Text.Json.Serialization;

namespace CodeWithMe.Core.Models.Envelope;

public sealed record ApiEnvelope<T>(
    T Data,
    [property: JsonPropertyName("meta")] PageMeta? Meta = null,
    [property: JsonPropertyName("links")] PageLinks? Links = null
);
