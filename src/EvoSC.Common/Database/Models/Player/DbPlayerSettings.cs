using EvoSC.Common.Interfaces.Models;
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Player;

[Table(TableName)]
public class DbPlayerSettings : IPlayerSettings
{
    public const string TableName = "PlayerSettings";
    
    [Column]
    public long PlayerId { get; set; }
    
    [Column]
    public string DisplayLanguage { get; set; }
}
