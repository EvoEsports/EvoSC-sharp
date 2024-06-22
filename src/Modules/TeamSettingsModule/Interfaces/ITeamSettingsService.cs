using System.Collections.Specialized;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.TeamSettingsModule.Models;

namespace EvoSC.Modules.Official.TeamSettingsModule.Interfaces;

public interface ITeamSettingsService
{
    public Task SetTeamSettingsAsync(TeamSettingsModel teamSettings);

    public Task<NameValueCollection> ParseClubLinkUrl(string clubLinkUrl);

    // public Task SetTeamSettingsFallbackAsync(string teamOneName, string teamTwoName);

    public Task<string> GetClubLinkUrl(
        string teamName,
        string primaryColor,
        string? secondaryColor = null,
        string? emblemUrl = null
    );

    public Task ShowTeamSettingsAsync(IPlayer player, TeamSettingsModel teamSettings);
}
