using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.SetName.Interfaces;

public interface ISetNameService
{
    /// <summary>
    /// Change the nickname of a player and trigger the event along with it.
    /// </summary>
    /// <param name="player">The player to change the nickname for.</param>
    /// <param name="newName">The new name of the player.</param>
    /// <returns></returns>
    public Task SetNicknameAsync(IPlayer player, string newName);
}
