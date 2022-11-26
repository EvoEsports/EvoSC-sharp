namespace EvoSC.Common.Models;

public class MapObject
{
    public Map Map { get; }
    public Stream MapStream { get; }

    public MapObject(Map map, Stream mapStream)
    {
        Map = map;
        MapStream = mapStream;
    }
}
