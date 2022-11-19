using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace EvoSC.Common.Database.Models.Maps;

[Dapper.Contrib.Extensions.Table("MapFavorites")]
public class DbMapFavorite
{
    [Key]
    public long Id { get; set; }
    
    [ForeignKey("Players")]
    public DbPlayer Player { get; set; }

    [ForeignKey("Maps")]
    public DbMap Map { get; set; }
}
