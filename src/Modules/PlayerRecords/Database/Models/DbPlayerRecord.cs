using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;
using LinqToDB.Mapping;

namespace EvoSC.Modules.Official.PlayerRecords.Database.Models;

[Table("PlayerRecords")]
public class DbPlayerRecord : IPlayerRecord
{
    [PrimaryKey, Identity]
    public long Id { get; set; }
    
    [Column]
    public long PlayerId { get; set; }
    
    [Column]
    public long? MapId { get; set; }
    
    [Column]
    public int Score { get; set; }
    
    [Column]
    public PlayerRecordType RecordType { get; set; }
    
    [Column]
    public string? Checkpoints { get; set; }
    
    [Column]
    public DateTime CreatedAt { get; set; }
    
    [Column]
    public DateTime UpdatedAt { get; set; }

    [Association(ThisKey = nameof(PlayerId), OtherKey = nameof(Common.Database.Models.Player.DbPlayer.Id))]
    public DbPlayer DbPlayer;

    public IPlayer Player => DbPlayer;

    [Association(ThisKey = nameof(MapId), OtherKey = nameof(Common.Database.Models.Maps.DbMap.Id))]
    public DbMap DbMap;

    public IMap? Map => DbMap;
}
