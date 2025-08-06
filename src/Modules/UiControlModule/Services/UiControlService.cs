using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Database.Repository;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.UiControlModule.Interfaces;
using LinqToDB;

namespace EvoSC.Modules.Official.UiControlModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class UiControlService(
    IManialinkManager manialinkManager,
    IModuleManager moduleManager,
    IPlayerCacheService playerCache,
    IDbConnectionFactory dbConnFactory
) : DbRepository(dbConnFactory), IUiControlService
{
    private const string ConfigMenuTemplate = "UiControlModule.Menu";

    public async Task DisplayMenuAsync(IOnlinePlayer player)
    {
        var hiddenModules = new List<string>();

        if (player.Settings.HiddenManialinks != null)
        {
            hiddenModules.AddRange(player.Settings.GetHiddenManialinks());
        }

        await manialinkManager.SendManialinkAsync(player, ConfigMenuTemplate,
            new { hiddenModules, moduleNames = GetTemplateNames() });
    }

    public List<string> GetTemplateNames()
    {
        return moduleManager.LoadedModules
            .Select(moduleLoadContext => moduleLoadContext.ManialinkTemplates)
            .SelectMany(a => a)
            .Where(manialinkTemplate => !manialinkTemplate.Name.StartsWith("UiControl"))
            .Where(manialinkTemplate => manialinkTemplate.Name.Split('.').Length <= 2)
            .Select(manialinkTemplate => manialinkManager.GetEffectiveName(manialinkTemplate.Name))
            .Order()
            .ToList();
    }

    public async Task SaveSettingsAsync(IOnlinePlayer player, List<string> hiddenManialinks)
    {
        player.Settings.SetHiddenManialinks(hiddenManialinks);

        await Table<DbPlayerSettings>()
            .Where(dbPlayer => dbPlayer.PlayerId == player.Id)
            .Set(settings => settings.HiddenManialinks, player.Settings.HiddenManialinks)
            .UpdateAsync();

        await manialinkManager.HideManialinkAsync(player, ConfigMenuTemplate);
        await playerCache.UpdatePlayerAsync(player);

        hiddenManialinks.ForEach(templateName => manialinkManager.HideManialinkAsync(player, templateName));
    }
}
