using EvoSC.Modules.Official.MapListModule.Interfaces.Models;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
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

    [Association(ThisKey = nameof(MapId), OtherKey = nameof(DbMap.Id))]
    public DbMap? Map { get; set; }

    [Association(ThisKey = nameof(PlayerId), OtherKey = nameof(DbPlayer.Id))]
    public DbPlayer? Player { get; set; }
}
