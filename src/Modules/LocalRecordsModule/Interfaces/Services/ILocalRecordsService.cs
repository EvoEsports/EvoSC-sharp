namespace EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;

public interface ILocalRecordsService
{
    public Task ShowWidgetAsync();
    public Task<IEnumerable<ILocalRecord>> GetLocalsOfCurrentMapAsync();
}
