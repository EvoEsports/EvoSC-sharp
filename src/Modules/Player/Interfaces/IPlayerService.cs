using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.Player.Interfaces;

public interface IPlayerService
{
    /// <summary>
    /// Creates the player if they don't exist, or updates them.
    /// Then greets the player in the chat.
    /// </summary>
    /// <param name="login">Login of the player that just joined.</param>
    /// <returns></returns>
    public Task UpdateAndGreetPlayerAsync(string login);
    
    /// <summary>
    /// Kick a player from the server.
    /// </summary>
    /// <param name="player">Player to kick.</param>
    /// <param name="actor">The player that triggered the action.</param>
    /// <returns></returns>
    public Task KickAsync(IPlayer player, IPlayer actor);
    
    /// <summary>
    /// Mute a player from the chat.
    /// </summary>
    /// <param name="player">Player to mute.</param>
    /// <param name="actor">The player that triggered the action.</param>
    /// <returns></returns>
    public Task MuteAsync(IPlayer player, IPlayer actor);
    
    /// <summary>
    /// Unmute a player from the chat.
    /// </summary>
    /// <param name="player">Player to unmute.</param>
    /// <param name="actor">The player that triggered the action.</param>
    /// <returns></returns>
    public Task UnmuteAsync(IPlayer player, IPlayer actor);
    
    /// <summary>
    /// Ban and blacklist a player from the server.
    /// </summary>
    /// <param name="player">Player to ban.</param>
    /// <param name="actor">The player that triggered the action.</param>
    /// <returns></returns>
    public Task BanAsync(IPlayer player, IPlayer actor);
    
    /// <summary>
    /// Unban and un-blacklist a player from the server.
    /// </summary>
    /// <param name="login">Login of the player to unban.</param>
    /// <param name="actor">The player that triggered the action.</param>
    /// <returns></returns>
    public Task UnbanAsync(string login, IPlayer actor);

    /// <summary>
    /// Force a player to spectator.
    /// </summary>
    /// <param name="player">The player to force to spectator.</param>
    /// <returns></returns>
    public Task ForceSpectatorAsync(IPlayer player);
}
