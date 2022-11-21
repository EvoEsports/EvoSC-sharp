namespace EvoSC.Common.Interfaces.Models;

public interface IPlayerFlags
{
    public bool ForceSpectator { get; }
    public bool ForceSpectatorSelectable { get; }
    public int StereoDisplayMode { get; }
    public bool IsManagedByAnOtherServer { get; }
    public bool IsServer { get; }
    public bool HasPlayerSlot { get; }
    public bool IsBroadcasting { get; }
    public bool HasJoinedGame { get; }
}
