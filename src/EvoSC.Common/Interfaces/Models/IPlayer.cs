using EvoSC.Common.Util;

namespace EvoSC.Common.Interfaces.Models;

/// <summary>
/// Represents an online or offline player.
/// </summary>
public interface IPlayer : IEquatable<IPlayer>
{
    /// <summary>
    /// The ID of the player.
    /// </summary>
    public long Id { get; }
    
    /// <summary>
    /// The Player's account ID.
    /// </summary>
    public string  AccountId { get; }
    
    /// <summary>
    /// The player's nickname
    /// </summary>
    public string NickName { get; }

    /// <summary>
    /// The nickname but stripped for all formatting. NOTE: not constant time.
    /// </summary>
    public string StrippedNickName => FormattingUtils.CleanTmFormatting(NickName);
    
    /// <summary>
    /// The known ubisoft name of the player, may change.
    /// </summary>
    public string UbisoftName { get; }
    
    /// <summary>
    /// The zone/path of the player's location.
    /// </summary>
    public string? Zone { get; }
    
    public IPlayerSettings Settings { get; }
    
    public IEnumerable<IGroup> Groups { get; }
    public IGroup? DisplayGroup { get; }
}
