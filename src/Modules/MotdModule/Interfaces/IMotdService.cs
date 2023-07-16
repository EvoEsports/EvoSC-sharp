using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MotdModule.Interfaces;

public interface IMotdService
{
    public Task ShowAsync(IPlayer player);
    public Task ShowEditAsync(IPlayer player);
    public void SetMotdSource(bool local, IPlayer player);
    public void SetLocalMotd(string text, IPlayer player);
    public Task<string> GetMotdAsync();
    public Task<IMotdEntry?> GetEntryAsync(IPlayer player);
    public Task InsertOrUpdateEntryAsync(IPlayer player, bool hidden);
    public void SetInterval(int interval, IPlayer player);
    public void SetUrl(string url, IPlayer player);
}
