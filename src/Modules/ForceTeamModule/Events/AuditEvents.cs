using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.ForceTeamModule.Events;

public enum AuditEvents
{
    /// <summary>
    /// Triggered when a player is forced switched
    /// to another team.
    /// </summary>
    [Identifier(Name = "ForceTeam:PlayerSwitched")]
    PlayerSwitched,
}
