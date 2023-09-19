using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Models.Players;

public class PlayerSpectatorStatus : IPlayerSpectatorStatus
{
    public required bool IsSpectator { get; init; }
    public required bool IsTemporarySpectator { get; init; }
    public required bool IsPureSpectator { get; init; }
    public required bool AutoTarget { get; init; }
    public required int CurrentTargetId { get; init; }
}
