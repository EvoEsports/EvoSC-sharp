using System.Collections.Specialized;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.TeamSettingsModule.Models;

namespace EvoSC.Modules.Official.TeamSettingsModule.Interfaces;

public interface ITeamSettingsService
{
    /// <summary>
    /// Applies the given team settings.
    /// </summary>
    /// <returns>A TeamSettingsModel containing the currently used team settings.</returns>
    public Task<TeamSettingsModel> GetCurrentTeamSettingsModel();
    
    /// <summary>
    /// Applies the given team settings.
    /// </summary>
    /// <param name="teamSettings">The TeamSettingsModel containing the settings.</param>
    /// <returns></returns>
    public Task SetTeamSettingsAsync(TeamSettingsModel teamSettings);

    /// <summary>
    /// Takes a club link URL and parses its parts to a name-value collection.
    /// </summary>
    /// <param name="clubLinkUrl">The club link URL.</param>
    /// <returns>A NameValueCollection containing the team settings from the given URL.</returns>
    public Task<NameValueCollection> ParseClubLinkUrl(string clubLinkUrl);

    /// <summary>
    /// Generates a club link URL for the given values.
    /// </summary>
    /// <param name="teamName">The name for the team.</param>
    /// <param name="primaryColor">The primary team color.</param>
    /// <param name="secondaryColor">The secondary team color.</param>
    /// <param name="emblemUrl">An image to be displayed as the emblem of the team.</param>
    /// <returns>The club link URL as string.</returns>
    public Task<string> GenerateClubLinkUrl(
        string teamName,
        string primaryColor,
        string? secondaryColor = null,
        string? emblemUrl = null
    );

    /// <summary>
    /// Displays the team settings window to the given player.
    /// </summary>
    /// <param name="player">The player that should see the team settings.</param>
    /// <param name="teamSettings">The TeamSettingsModel with the current settings.</param>
    /// <returns></returns>
    public Task ShowTeamSettingsAsync(IPlayer player, TeamSettingsModel teamSettings);

    /// <summary>
    /// Hides the team settings for the given player.
    /// </summary>
    /// <param name="player">The player where team settings should be closed.</param>
    /// <returns></returns>
    public Task HideTeamSettingsAsync(IPlayer player);
}
