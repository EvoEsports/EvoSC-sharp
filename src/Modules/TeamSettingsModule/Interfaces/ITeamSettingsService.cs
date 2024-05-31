using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.TeamSettingsModule.Interfaces;

public interface ITeamSettingsService
{
    /// <summary>
    /// Displays the team settings UI to the given player.
    /// </summary>
    public Task ShowTeamSettingsAsync(IPlayer player);
    
    /// <summary>
    /// Updates the team settings.
    /// </summary>
    // public Task UpdateTeamSettingsAsync(IPlayer player, object settings);
}
