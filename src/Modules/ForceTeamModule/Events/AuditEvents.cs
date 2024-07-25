using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.ForceTeamModule.Events;

public enum AuditEvents
{
    /// <summary>
    /// Triggered when a player tries to confirm map deletion.
    /// </summary>
    [Identifier(Name = "ForceTeam:PlayerSwitched")]
    PlayerSwitched,
}
