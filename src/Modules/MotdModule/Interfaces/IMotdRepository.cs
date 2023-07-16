using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MotdModule.Database.Models;

namespace EvoSC.Modules.Official.MotdModule.Interfaces;

public interface IMotdRepository
{
    /// <summary>
    /// Gets the <see cref="MotdEntry" /> of the player
    /// </summary>
    /// <param name="player">The <see cref="IPlayer"/> of which the <see cref="MotdEntry"/> should be fetched.</param>
    /// <returns>The <see cref="MotdEntry"/> of the specified player.</returns>
    public Task<MotdEntry?> GetEntryAsync(IPlayer player);
    
    /// <summary>
    /// Inserts a <see cref="MotdEntry"/> to the database.
    /// </summary>
    /// <param name="player">The <see cref="IPlayer"/> that should be inserted.</param>
    /// <param name="hidden">Whether or not the MOTD should be hidden</param>
    /// <returns>The inserted <see cref="MotdEntry"/></returns>
    public Task<MotdEntry> InsertOrUpdateEntryAsync(IPlayer player, bool hidden);
}
