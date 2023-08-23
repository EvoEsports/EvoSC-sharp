namespace EvoSC.Common.Models.Maps;

public class MapMetadata
{
    public required string MapUid { get; init; }
    public required string MapName { get; init; }
    public required string AuthorId { get; init; }
    public required string AuthorName { get; init; }
    public required string? ExternalId { get; init; }
    public required DateTime? ExternalVersion { get; init; }
    public required MapProviders? ExternalMapProvider { get; init; }
}
