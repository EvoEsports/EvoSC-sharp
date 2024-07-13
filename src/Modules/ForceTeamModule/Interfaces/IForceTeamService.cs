using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.ForceTeamModule.Interfaces;

public interface IForceTeamService
{
    public Task ShowWindowAsync(IPlayer player);
    
    public Task BalanceTeamsAsync();
}
