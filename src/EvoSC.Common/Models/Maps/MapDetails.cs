using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;

namespace EvoSC.Common.Models.Maps;

public class MapDetails : IMapDetails
{
    public IRaceTime AuthorTime { get; set; }
    public IRaceTime GoldTime { get; set; }
    public IRaceTime SilverTime { get; set; }
    public IRaceTime BronzeTime { get; set; }
    public string Environment { get; set; }
    public string Mood { get; set; }
    public int Cost { get; set; }
    public bool MultiLap { get; set; }
    public int LapCount { get; set; }
    public string MapStyle { get; set; }
    public string MapType { get; set; }
    public int CheckpointCount { get; set; }
    public IMap Map { get; set; }
}
