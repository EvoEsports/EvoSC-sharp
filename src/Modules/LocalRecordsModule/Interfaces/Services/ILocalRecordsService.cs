using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;

public interface ILocalRecordsService
{
    public Task<IEnumerable<ILocalRecord>> GetLocalsOfCurrentMapAsync();
    public Task ShowWidgetAsync(IPlayer player);
    public Task ShowWidgetToAllAsync();
}
