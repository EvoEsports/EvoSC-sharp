using System.ComponentModel.DataAnnotations.Schema;

namespace EvoSC.Modules.Official.MapListModule.Database.Models;

[Table(TableName)]
public class DbMapTagMap
{
    public const string TableName = "MapTagMap";
    
    [Column]
    public int MapTagId { get; set; }
    
    [Column]
    public int MapId { get; set; }
}
