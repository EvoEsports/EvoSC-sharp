using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.MotdModule.Interfaces;

namespace EvoSC.Modules.Official.MotdModule.Controllers;

[Controller]
public class MotdManialinkController : ManialinkController
{
    private const string Template = "MotdModule.MotdTemplate";
    
    private readonly IMotdService _motdService;

    public MotdManialinkController(IMotdService motdService)
    {
        _motdService = motdService;
    }

    public async Task CloseAsync(bool isChecked)
    {
        await HideAsync(Context.Player, Template);
        await _motdService.InsertOrUpdateEntryAsync(Context.Player, isChecked);
    }
}
