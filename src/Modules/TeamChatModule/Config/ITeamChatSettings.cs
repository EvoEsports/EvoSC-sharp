using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.TeamChatModule.Config;

[Settings]
public interface ITeamChatSettings
{
    [Option(DefaultValue = 1L), Description("Show team chat to players in this group, even if they are not in the team. Has higher priority than ExcludeGroup.")]
    public long IncludeGroup { get; }
    
    [Option(DefaultValue = 0L), Description("Do not show team chat to players in this group, even if they are in the team.")]
    public long ExcludeGroup { get; }
}
