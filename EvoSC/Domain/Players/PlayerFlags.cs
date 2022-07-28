namespace EvoSC.Domain.Players;

public record PlayerFlags
{
    private readonly int _flags;
    
    public int ForceSpectator => _flags % 10;
    public bool IsReferee => _flags / 10 % 10 == 1;
    public bool IsPodiumReady => _flags / 100 % 10 == 1;
    public int StereoDisplayMode => _flags / 1000 % 10;
    public bool IsManagedByAnOtherServer => _flags / 10000 % 10 == 1;
    public bool IsServer => _flags / 100000 % 10 == 1;
    public bool HasPlayerSlot => _flags / 1000000 % 10 == 1;
    public bool IsBroadcasting => _flags / 10000000 % 10 == 1;
    public bool HasJoinedGame => _flags / 100000000 % 10 == 1;

    public PlayerFlags(int flags) => _flags = flags;
}
