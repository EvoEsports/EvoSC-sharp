using EvoSC.Modules.Official.MapListModule.Interfaces.Models;
using LinqToDB.Mapping;

namespace EvoSC.Modules.Official.MapListModule.Database.Models;

[Table(TableName)]
public class DbMapTag : IMapTag
{
    public const string TableName = "MapTags";

    [PrimaryKey, Identity]
    public int Id { get; set; }

    [Column]
    public string Name { get; set; } = "";
}
