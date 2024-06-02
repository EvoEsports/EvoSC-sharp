using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Util;
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Maps;

[Table(TableName)]
public class DbMapDetails : IMapDetails
{
    public const string TableName = "MapDetails";
    
    [Column(nameof(AuthorTime))]
    public int DbAuthorTime { get; set; }
    
    [Column(nameof(GoldTime))]
    public int DbGoldTime { get; set; }
    
    [Column(nameof(SilverTime))]
    public int DbSilverTime { get; set; }
    
    [Column(nameof(BronzeTime))]
    public int DbBronzeTime { get; set; }
    
    [Association(ThisKey = nameof(MapId), OtherKey = nameof(Maps.DbMap.Id))]
    public DbMap DbMap { get; set; }

    public IRaceTime AuthorTime
    {
        get { return RaceTime.FromMilliseconds(DbAuthorTime); }
        set { DbAuthorTime = value.TotalMilliseconds; }
    }

    public IRaceTime GoldTime
    {
        get { return RaceTime.FromMilliseconds(DbGoldTime); }
        set { DbGoldTime = value.TotalMilliseconds; }
    }
    
    public IRaceTime SilverTime
    {
        get { return RaceTime.FromMilliseconds(DbSilverTime); }
        set { DbSilverTime = value.TotalMilliseconds; }
    }
    
    public IRaceTime BronzeTime
    {
        get { return RaceTime.FromMilliseconds(DbBronzeTime); }
        set { DbBronzeTime = value.TotalMilliseconds; }
    }
    
    [PrimaryKey]
    public int MapId { get; set; }
    
    [Column]
    public string Environment { get; set; }
    
    [Column]
    public string Mood { get; set; }
    
    [Column]
    public int Cost { get; set; }
    
    [Column]
    public bool MultiLap { get; set; }
    
    [Column]
    public int LapCount { get; set; }
    
    [Column]
    public string MapStyle { get; set; }
    
    [Column]
    public string MapType { get; set; }
    
    [Column]
    public int CheckpointCount { get; set; }

    public IMap Map => DbMap;
}
