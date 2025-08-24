using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.UiControlModule.Interfaces;
using EvoSC.Modules.Official.UiControlModule.Models;

namespace EvoSC.Modules.Official.UiControlModule.Controllers;

[Controller]
public class UiControlManialinkController(IUiControlService uiControlService) : ManialinkController
{
    public async Task SaveConfigurationAsync(UiControlEntryModel formData)
    {
        string[] hiddenManialinks = [];

        if (formData.HiddenManialinks.Length > 0)
        {
            hiddenManialinks = formData.HiddenManialinks.Split("|");
        }

        await uiControlService.SaveSettingsAsync(Context.Player, hiddenManialinks);
    }

    public Task HideAllAsync(UiControlEntryModel formData) =>
        uiControlService.SaveSettingsAsync(Context.Player, uiControlService.GetTemplateNames());
}
