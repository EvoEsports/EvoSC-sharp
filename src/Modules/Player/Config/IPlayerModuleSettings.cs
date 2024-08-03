using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.Player.Config;

[Settings]
public interface IPlayerModuleSettings
{
    [Option(DefaultValue = true), Description("Automatically add new players to the default group.")]
    public bool AddToDefaultGroup { get; set; }
    
    [Option(DefaultValue = 2), Description("The ID of the group new players will be added to.")]
    public int DefaultGroupId { get; set; }
}
