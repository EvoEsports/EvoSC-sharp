using EvoSC.Modules.Official.MapListModule.Interfaces.Models;
using LinqToDB.Mapping;

namespace EvoSC.Modules.Official.MapListModule.Database.Models;

[Table(TableName)]
public class DbMapFavorite : IMapFavorite
{
    public const string TableName = "MapFavorites";

    [PrimaryKey, Identity]
    public long Id { get; set; }

    [Column]
    public long MapId { get; set; }

    [Column]
    public long PlayerId { get; set; }
}
