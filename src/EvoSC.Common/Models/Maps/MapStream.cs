namespace EvoSC.Common.Models.Maps;

/// <summary>
/// Contains the map file and the map metadata which the map service requires in order to add the map to the database
/// and the server.
/// </summary>
public class MapStream(MapMetadata mapMetadata, Stream mapFile)
{
    public MapMetadata MapMetadata { get; } = mapMetadata;
    public Stream MapFile { get; } = mapFile;
}
