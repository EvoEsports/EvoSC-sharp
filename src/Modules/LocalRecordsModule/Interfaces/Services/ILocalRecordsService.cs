using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;

public interface ILocalRecordsService
{
    public Task<ILocalRecord[]> GetLocalsOfCurrentMapFromPosAsync();
    public Task ShowWidgetAsync(IPlayer player);
    public Task ShowWidgetToAllAsync();
    public Task UpdatePbAsync(IPlayerRecord record);

    public Task ResetLocalRecordsAsync();
}
