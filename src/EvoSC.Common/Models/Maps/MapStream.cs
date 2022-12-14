namespace EvoSC.Common.Models.Maps;

/// <summary>
/// Contains the map file and the map metadata which the map service requires in order to add the map to the database
/// and the server.
/// </summary>
public class MapStream
{
    public MapMetadata MapMetadata { get; }
    public Stream MapFile { get; }

    public MapStream(MapMetadata mapMetadata, Stream mapFile)
    {
        MapMetadata = mapMetadata;
        MapFile = mapFile;
    }
}
