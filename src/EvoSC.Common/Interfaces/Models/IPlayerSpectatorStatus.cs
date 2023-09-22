namespace EvoSC.Common.Interfaces.Models;

public interface IPlayerSpectatorStatus
{
    public bool IsSpectator { get; }
    public bool IsTemporarySpectator { get; }
    public bool IsPureSpectator { get; }
    public bool AutoTarget { get; }
    public int CurrentTargetId { get; }
}
