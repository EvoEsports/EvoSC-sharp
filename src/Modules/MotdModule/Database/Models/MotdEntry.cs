using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using LinqToDB.Mapping;

namespace EvoSC.Modules.Official.MotdModule.Database.Models;

[Table("Motd")]
public class MotdEntry : IMotdEntry
{
    [PrimaryKey, Column]
    public long PlayerId { get; set; }
    
    [Column]
    public bool Hidden { get; set; }
    
    [Association(ThisKey = nameof(PlayerId), OtherKey = nameof(Common.Database.Models.Player.DbPlayer.Id))]
    public DbPlayer DbPlayer { get; set; }
    
    public IPlayer Player => DbPlayer;
}
