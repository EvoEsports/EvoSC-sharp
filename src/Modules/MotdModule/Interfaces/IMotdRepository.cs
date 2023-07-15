using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MotdModule.Database.Models;

namespace EvoSC.Modules.Official.MotdModule.Interfaces;

public interface IMotdRepository
{
    public Task<MotdEntry?> GetEntryAsync(IPlayer player);
    
    public Task<MotdEntry> InsertOrUpdateEntryAsync(IPlayer player, bool hidden);
}
