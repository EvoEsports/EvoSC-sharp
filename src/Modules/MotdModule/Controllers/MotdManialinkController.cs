using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.MotdModule.Interfaces;

namespace EvoSC.Modules.Official.MotdModule.Controllers;

[Controller]
public class MotdManialinkController(IMotdService motdService) : ManialinkController
{
    private const string Template = "MotdModule.MotdTemplate";

    public async Task CloseAsync(bool isChecked)
    {
        await HideAsync(Context.Player, Template);
        await motdService.InsertOrUpdateEntryAsync(Context.Player, isChecked);
    }
}
