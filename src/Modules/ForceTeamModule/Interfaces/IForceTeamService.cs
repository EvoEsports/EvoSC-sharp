using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.ForceTeamModule.Interfaces;

public interface IForceTeamService
{
    /// <summary>
    /// Show the force team window.
    /// </summary>
    /// <param name="player">Player to show it to.</param>
    /// <returns></returns>
    public Task ShowWindowAsync(IPlayer player);
    
    /// <summary>
    /// Start automatic team balance.
    /// </summary>
    /// <returns></returns>
    public Task BalanceTeamsAsync();
    
    /// <summary>
    /// Switch a player to the other team.
    /// </summary>
    /// <param name="player">The player to switch.</param>
    /// <returns></returns>
    public Task<PlayerTeam> SwitchPlayerAsync(IOnlinePlayer player);
}
