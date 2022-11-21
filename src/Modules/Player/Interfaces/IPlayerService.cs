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

    public Task KickAsync(IPlayer player);
}
