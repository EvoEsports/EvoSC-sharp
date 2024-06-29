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

    public DbPlayerRecord(){}

    public DbPlayerRecord(IPlayerRecord? record)
    {
        if (record == null)
        {
            return;
        }

        Id = record.Id;
        PlayerId = record.Player.Id;
        MapId = record.Map.Id;
        Score = record.Score;
        RecordType = record.RecordType;
        Checkpoints = record.Checkpoints;
        CreatedAt = record.CreatedAt;
        UpdatedAt = record.UpdatedAt;
        DbPlayer = new DbPlayer(record.Player);
        DbMap = new DbMap(record.Map);
    }

    public int CompareTo(IPlayerRecord? other)
    {
        if (other == null)
        {
            // better than "nothing"
            return 1;
        }
        
        if (RecordType != other.RecordType)
        {
            throw new InvalidOperationException("Cannot compare records of different types");
        }

        return RecordType switch
        {
            PlayerRecordType.Points => ComparePoints(),
            PlayerRecordType.Time => CompareTime(),
            _ => CompareTime()
        };

        // Compare time record type, lower is better
        int CompareTime() => Score switch
        {
            _ when Score < other.Score => -1,
            _ when Score > other.Score => 1,
            _ => 0
        };

        // compare points record type, higher is better
        int ComparePoints() => Score switch
        {
            _ when Score > other.Score => -1,
            _ when Score < other.Score => 1,
            _ => 0
        };
    }
}
