using Dapper.Contrib.Extensions;

namespace EvoSC.Common.Database.Models;

[Table("Map_Records")]
public class DbMapRecord
{
    [Key]
    public int Id { get; set; }

    public int Score { get; set; }

    public int Rank { get; set; }

    public string Checkpoints { get; set; }

    public DbPlayer Player { get; set; }

    public DbMap Map { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
