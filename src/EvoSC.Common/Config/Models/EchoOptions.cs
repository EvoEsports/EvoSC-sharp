namespace EvoSC.Common.Config.Models;

/// <summary>
/// Control how to send messages and responses to the in-game chat.
/// </summary>
public enum EchoOptions
{
    /// <summary>
    /// Only send the message to the affecting player.
    /// </summary>
    Player,
    /// <summary>
    /// Send the message to all players.
    /// </summary>
    All,
    /// <summary>
    /// Don't send the message to anyone.
    /// </summary>
    None
}
