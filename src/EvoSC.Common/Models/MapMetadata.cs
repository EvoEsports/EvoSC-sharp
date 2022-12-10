namespace EvoSC.Common.Models;

public class MapMetadata
{
    public string MapUid { get; }
    public string MapName { get; }
    public string AuthorId { get; }
    public string AuthorName { get; }
    public long TmIoId { get; }
    public DateTime? TmIoVersion { get; }
    public long MxId { get; }
    public DateTime? MxVersion { get; }

    public MapMetadata(
        string mapUid,
        string mapName,
        string authorId,
        string authorName,
        long externalId,
        DateTime? externalVersion,
        bool isTmIo,
        bool isMx
    ) {
        MapUid = mapUid;
        MapName = mapName;
        AuthorId = authorId;
        AuthorName = authorName;

        if (isTmIo)
        {
            TmIoId = externalId;
            TmIoVersion = externalVersion;
        } else if (isMx)
        {
            MxId = externalId;
            MxVersion = externalVersion;
        }
    }
}
