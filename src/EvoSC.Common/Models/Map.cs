using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Models;

public class Map: IMap
{
    public long Id { get; set; }
    public string Uid { get; set; }
    public string Name { get; set; }
    public IPlayer Author { get; set; }
    public long ManiaExchangeId { get; set; }
    public DateTime? ManiaExchangeVersion { get; set; }
    public long TrackmaniaIoId { get; set; }
    public DateTime? TrackmaniaIoVersion { get; set; }

    public Map()
    {
        
    }

    public Map(DbMap dbMap) : this()
    {
        Id = dbMap.Id;
        Uid = dbMap.Uid;
        Name = dbMap.Name;
        Author = dbMap.Author;
        ManiaExchangeId = dbMap.ManiaExchangeId;
        ManiaExchangeVersion = dbMap.ManiaExchangeVersion;
        TrackmaniaIoId = dbMap.TrackmaniaIoId;
        TrackmaniaIoVersion = dbMap.TrackmaniaIoVersion;
    }
}
