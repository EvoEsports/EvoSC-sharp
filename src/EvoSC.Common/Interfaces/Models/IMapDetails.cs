using EvoSC.Common.Interfaces.Util;

namespace EvoSC.Common.Interfaces.Models;

public interface IMapDetails
{
    public IRaceTime AuthorTime { get; }
    public IRaceTime GoldTime { get; }
    public IRaceTime SilverTime { get; }
    public IRaceTime BronzeTime { get; }
    
    public string Environment { get; }
    public string Mood { get; }
    public int Cost { get; }
    public bool MultiLap { get; }
    public int LapCount { get; }
    public string MapStyle { get; }
    public string MapType { get; }
    public int CheckpointCount { get; }
    
    public IMap Map { get; }
}
