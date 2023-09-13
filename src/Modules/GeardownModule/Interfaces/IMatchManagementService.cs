using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces;

public interface IMatchManagementService
{
    public Task SetPlayerPointsAsync(IPlayer player, int points);
    public Task PauseMatchAsync();
    public Task UnpauseMatchAsync();
}
