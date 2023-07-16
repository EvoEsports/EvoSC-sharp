using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MotdModule.Interfaces;

public interface IMotdService
{
    public Task ShowAsync(IPlayer player);
    public Task<string> GetMotdAsync();
    public Task<IMotdEntry?> GetEntryAsync(IPlayer player);
    public Task InsertOrUpdateEntryAsync(IPlayer player, bool hidden);
    public void SetInterval(int interval);
    public void SetUrl(string url);
}
