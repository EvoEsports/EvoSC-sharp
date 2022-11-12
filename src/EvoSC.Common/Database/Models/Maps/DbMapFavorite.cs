using Dapper.Contrib.Extensions;

namespace EvoSC.Common.Database.Models;

[Table("Player_MapFavorites")]
public class DbMapFavorite
{
    [Key]
    public int Id { get; set; }

    public DbPlayer Player { get; set; }

    public DbMap Map { get; set; }
}
