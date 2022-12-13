namespace EvoSC.Common.Models.Maps;

public class MapMetadata
{
    public string MapUid { get; init; }
    public string MapName { get; init; }
    public string AuthorId { get; init; }
    public string AuthorName { get; init; }
    public string ExternalId { get; init; }
    public DateTime? ExternalVersion { get; init; }
    public MapProviders? ExternalMapProvider { get; init; }

    public MapMetadata(string mapUid, string mapName, string authorId, string authorName, string externalId,
        DateTime? externalVersion, MapProviders? externalMapProvider)
    {
        MapUid = mapUid;
        MapName = mapName;
        AuthorId = authorId;
        AuthorName = authorName;
        ExternalId = externalId;
        ExternalVersion = externalVersion;
        ExternalMapProvider = externalMapProvider;
    }
}
