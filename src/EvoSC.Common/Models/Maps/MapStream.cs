using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Models.Maps;

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
