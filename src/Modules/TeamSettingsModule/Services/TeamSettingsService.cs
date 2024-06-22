using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Models;

namespace EvoSC.Modules.Official.TeamSettingsModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Scoped)]
public class TeamSettingsService(IServerClient server, IManialinkManager manialinks, Locale locale) : ITeamSettingsService
{
    public async Task SetTeamSettingsAsync(TeamSettingsModel teamSettings)
    {
        await server.Remote.SetForcedClubLinksAsync(
            await GetClubLinkUrl(teamSettings.Team1Name, teamSettings.Team1PrimaryColor, teamSettings.Team1SecondaryColor, teamSettings.Team1EmblemUrl),
            await GetClubLinkUrl(teamSettings.Team2Name, teamSettings.Team2PrimaryColor, teamSettings.Team2SecondaryColor, teamSettings.Team2EmblemUrl)
        );
    }
    
    public Task<string> GetClubLinkUrl(string teamName, string primaryColor, string? secondaryColor = null,
        string? emblemUrl = null)
    {
        var url = new UriBuilder("https://club-link.evotm.workers.dev")
        {
            Query = $"name={Uri.EscapeDataString(teamName)}&primary={Uri.EscapeDataString(primaryColor)}"
        };

        if (secondaryColor != null)
        {
            url.Query += $"&secondary={Uri.EscapeDataString(secondaryColor)}";
        }

        if (emblemUrl != null)
        {
            url.Query += $"&emblem={Uri.EscapeDataString(emblemUrl)}";
        }

        return Task.FromResult(url.ToString());
    }

    // public async Task SetTeamSettingsFallbackAsync(string teamOneName, string teamTwoName)
    // {
    //     //'unused', 0., 'World', teamName1, teamColor1, team1Country, teamName2, teamColor2, team2Country
    //     await server.Remote.SetTeamInfoAsync(
    //         "invalid_team_check_index", 0.0, "World",
    //         teamOneName, 0.0, "World",
    //         teamTwoName, 0.0, "World"
    //     );
    // }

    public async Task ShowTeamSettingsAsync(IPlayer player, TeamSettingsModel teamSettings)
    {
        await manialinks.SendManialinkAsync(player, "TeamSettings.EditTeamSettings",
            new
            {
                Settings = teamSettings,
                Locale = locale
            }
        );
    }
}
