namespace EvoSC.Modules.Evo.GeardownModule.Interfaces;

public interface IGeardownService
{
    public Task SetupServerAsync(string matchToken);
}