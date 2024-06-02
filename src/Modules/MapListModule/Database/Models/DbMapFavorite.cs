using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces.Models;
using LinqToDB.Mapping;

namespace EvoSC.Modules.Official.MapListModule.Database.Models;

[Table(TableName)]
public class DbMapFavorite : IMapFavorite
{
    public const string TableName = "MapFavorites";
    
    [PrimaryKey, Identity]
    public int Id { get; set; }
    
    [Association(ThisKey = nameof(Id), OtherKey = nameof(DbMap.Id))]
    public IMap Map { get; set; }
    
    [Association(ThisKey = nameof(Id), OtherKey = nameof(DbPlayer.Id))]
    public IPlayer Player { get; set; }
}
