using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Models.Players;

public class PlayerFlags : IPlayerFlags
{
    public required bool ForceSpectator { get; init; }
    public required bool ForceSpectatorSelectable { get; init; }
    public required int StereoDisplayMode { get; init; }
    public required bool IsManagedByAnOtherServer { get; init; }
    public required bool IsServer { get; init; }
    public required bool HasPlayerSlot { get; init; }
    public required bool IsBroadcasting { get; init; }
    public required bool HasJoinedGame { get; init; }
}
