using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace EvoSC.Common.Database.Models.Maps;

[Dapper.Contrib.Extensions.Table("MapRecords")]
public class DbMapRecord
{
    [Key]
    public long Id { get; set; }
    
    public long PlayerId { get; set; }
    
    public long MapId { get; set; }

    public int Score { get; set; }

    public int Rank { get; set; }

    public string Checkpoints { get; set; }

    [ForeignKey("Players")]
    public DbPlayer Player { get; set; }

    [ForeignKey("Maps")]
    public DbMap Map { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
