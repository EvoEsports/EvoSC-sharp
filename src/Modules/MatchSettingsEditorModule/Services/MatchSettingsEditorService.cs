using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MatchSettingsEditorModule.Interfaces;

namespace EvoSC.Modules.Official.MatchSettingsEditorModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MatchSettingsEditorService(IManialinkManager manialinks, IMatchSettingsService matchSettingsService) : IMatchSettingsEditorService
{
    public async Task ShowEditorAsync(IOnlinePlayer player)
    {
        var matchSettingsList = await matchSettingsService.GetAllMatchSettingsAsync();
        await manialinks.SendManialinkAsync(player, "MatchSettingsEditorModule.MatchSettingsEditor",
            new { matchSettingsList });
    }
}
