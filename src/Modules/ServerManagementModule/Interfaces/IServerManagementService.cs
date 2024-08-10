namespace EvoSC.Modules.Official.ServerManagementModule.Interfaces;

public interface IServerManagementService
{
    public Task SetPasswordAsync(string password);
    public Task SetMaxPlayersAsync(int maxPlayers);
    public Task SetMaxSpectatorsAsync(int maxSpectators);
}
