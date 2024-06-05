namespace EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;

public interface ILocalRecordsService
{
    public Task<IEnumerable<ILocalRecord>> GetLocalsOfCurrentMapAsync();
}
