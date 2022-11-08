namespace EvoSC.Common.Interfaces.Models;

public interface IPlayer : IUser
{
    /// <summary>
    /// The Player's account ID.
    /// </summary>
    public string AccountId { get; }
    /// <summary>
    /// The player's nickname
    /// </summary>
    public string NickName { get; }
    /// <summary>
    /// The known ubisoft name of the player, may change.
    /// </summary>
    public string UbisoftName { get; }
    /// <summary>
    /// The zone/path of the player's location.
    /// </summary>
    public string Zone { get; }
}
