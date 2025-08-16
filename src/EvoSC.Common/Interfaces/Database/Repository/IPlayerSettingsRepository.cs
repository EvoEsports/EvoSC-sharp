using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Interfaces.Database.Repository;

public interface IPlayerSettingsRepository
{
    /// <summary>
    /// Updates the hidden manialinks setting of the player.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="hiddenManialinks"></param>
    /// <returns></returns>
    public Task UpdateHiddenManialinksAsync(IPlayer player, IEnumerable<string> hiddenManialinks);
}
