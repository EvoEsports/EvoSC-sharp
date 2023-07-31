using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.MotdModule.Interfaces;

namespace EvoSC.Modules.Official.MotdModule.Controllers;

[Controller]
public class MotdEditManialinkController : ManialinkController
{
    private readonly IMotdService _motdService;

    public MotdEditManialinkController(IMotdService motdService)
    {
        _motdService = motdService;
    }
    
    public async Task SaveAsync(string text)
    {
        _motdService.SetLocalMotd(text, Context.Player);
        await HideAsync(Context.Player, "MotdModule.MotdEdit");
    }
}
