using System.Collections.Specialized;
using System.Web;
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
public class TeamSettingsService(IServerClient server, IManialinkManager manialinks, Locale locale)
    : ITeamSettingsService
{
    private const string ClubLinkGeneratorUrl = "https://club-link.evotm.workers.dev";
    public const string DefaultTeam1Name = "Blue";
    public const string DefaultTeam2Name = "Red";
    public const string DefaultTeam1Color = "00f";
    public const string DefaultTeam2Color = "f00";

    public async Task SetTeamSettingsAsync(TeamSettingsModel teamSettings)
    {
        var clubLinkTeam1 = await GetClubLinkUrl(teamSettings.Team1Name, teamSettings.Team1PrimaryColor,
            teamSettings.Team1SecondaryColor, teamSettings.Team1EmblemUrl);
        var clubLinkTeam2 = await GetClubLinkUrl(teamSettings.Team2Name, teamSettings.Team2PrimaryColor,
            teamSettings.Team2SecondaryColor, teamSettings.Team2EmblemUrl);

        await server.Remote.SetForcedClubLinksAsync(clubLinkTeam1, clubLinkTeam2);
    }

    public Task<NameValueCollection> ParseClubLinkUrl(string clubLinkUrl)
    {
        var url = new UriBuilder(clubLinkUrl);
        var queryValues = HttpUtility.ParseQueryString(url.Query);

        return Task.FromResult(queryValues);
    }

    public Task<string> GetClubLinkUrl(string teamName, string primaryColor, string? secondaryColor = null,
        string? emblemUrl = null)
    {
        var url = new UriBuilder(ClubLinkGeneratorUrl)
        {
            Query = $"name={Uri.EscapeDataString(teamName)}&primary={Uri.EscapeDataString(primaryColor)}"
        };

        if (secondaryColor is { Length: > 0 })
        {
            url.Query += $"&secondary={Uri.EscapeDataString(secondaryColor)}";
        }

        if (emblemUrl != null)
        {
            url.Query += $"&emblem={Uri.EscapeDataString(emblemUrl)}";
        }

        return Task.FromResult(url.ToString());
    }

    public async Task ShowTeamSettingsAsync(IPlayer player, TeamSettingsModel teamSettings)
    {
        await manialinks.SendManialinkAsync(player, "TeamSettings.EditTeamSettings",
            new { Settings = teamSettings, Locale = locale }
        );
    }
}
