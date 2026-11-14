using EvoSC.Common.Database.Models.Maps;
using LinqToDB.Mapping;

namespace EvoSC.Modules.Official.MapListModule.Database.Models;

[Table(TableName)]
public class DbMapTagMap
{
    public const string TableName = "MapTagMap";

    [Column]
    public int MapTagId { get; set; }

    [Column]
    public long MapId { get; set; }

    [Association(ThisKey = nameof(MapTagId), OtherKey = nameof(DbMapTag.Id))]
    public DbMapTag? MapTag { get; set; }

    [Association(ThisKey = nameof(MapId), OtherKey = nameof(DbMap.Id))]
    public DbMap? Map { get; set; }
}
