using System.ComponentModel.DataAnnotations.Schema;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;
using SqlKata;

namespace EvoSC.Modules.Official.PlayerRecords.Database.Models;

[Table("PlayerRecords")]
public class DbPlayerRecord : IPlayerRecord
{
    [Key]
    public long Id { get; set; }
    public long PlayerId { get; set; }
    public long? MapId { get; set; }
    public int Score { get; set; }
    public PlayerRecordType RecordType { get; set; }
    public string? Checkpoints { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    [Ignore]
    public IPlayer Player { get; set; }
    [Ignore]
    public IMap? Map { get; set; }
}
