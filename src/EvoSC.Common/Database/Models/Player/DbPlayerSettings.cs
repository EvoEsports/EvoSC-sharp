using EvoSC.Common.Interfaces.Models;
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Player;

[Table("PlayerSettings")]
public class DbPlayerSettings : IPlayerSettings
{
    [Column]
    public long PlayerId { get; set; }
    
    [Column]
    public string DisplayLanguage { get; set; }
}
