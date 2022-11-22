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
    public Task UpdateAndGreetPlayer(string login);

    public Task KickAsync(IPlayer player, IPlayer actor);
    public Task MuteAsync(IPlayer player, IPlayer actor);
    public Task UnmuteAsync(IPlayer player, IPlayer actor);
    public Task BanAsync(IPlayer player, IPlayer actor);
}
