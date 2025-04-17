using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MatchSettingsEditorModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MatchSettingsEditorModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MatchSettingsEditorService(
    IManialinkManager manialinks,
    IMatchSettingsService matchSettingsService,
    ILogger<MatchSettingsEditorService> logger)
    : IMatchSettingsEditorService
{
    public async Task ShowOverviewAsync(IOnlinePlayer player)
    {
        try
        {
            var matchSettingsList = await matchSettingsService.GetAllMatchSettingsAsync();
            await manialinks.SendManialinkAsync(player, "MatchSettingsEditorModule.MatchSettingsOverview",
                new { matchSettingsList });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to show matchsettings overview");
            throw;
        }
    }

    public async Task ShowEditorAsync(IOnlinePlayer player, string name)
    {
        try
        {
            var matchSettings = await matchSettingsService.GetMatchSettingsAsync(name);
            await manialinks.SendManialinkAsync(player, "MatchSettingsEditorModule.MatchSettingsEditor",
                new { matchSettings });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to show matchsettings editor for '{Name}'", name);
        }
    }
}
