using Dapper.Contrib.Extensions;

namespace EvoSC.Common.Database.Models;

[Table("Map_Karma")]
public class DbMapKarma
{
    [Key]
    public int Id { get; set; }

    public int Rating { get; set; }

    public bool New { get; set; }

    public DbMap Map { get; set; }

    public DbPlayer Player { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
