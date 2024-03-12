using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces.Models;
using LinqToDB.Mapping;

namespace EvoSC.Modules.Official.MapListModule.Database.Models;

[System.ComponentModel.DataAnnotations.Schema.Table(TableName)]
public class DbMapTag : IMapTag
{
    public const string TableName = "MapTags";
    
    [PrimaryKey, Identity]
    public int Id { get; set; }
    
    [Column]
    public string Name { get; set; }
    
    [Association(ThisKey = nameof(DbMapTagMap.MapTagId), OtherKey = nameof(DbMapTagMap.MapId))]
    public IEnumerable<IMap> Maps { get; set; }
}
