using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace EvoSC.Common.Database.Models.Maps;

[Dapper.Contrib.Extensions.Table("MapKarma")]
public class DbMapKarma
{
    [Key]
    public long Id { get; set; }

    public int Rating { get; set; }

    public bool New { get; set; }

    [ForeignKey("Maps")]
    public DbMap Map { get; set; }

    [ForeignKey("Players")]
    public DbPlayer Player { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
