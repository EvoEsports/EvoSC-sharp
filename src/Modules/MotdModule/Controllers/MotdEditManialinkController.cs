using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Modules.Official.MotdModule.Models;

namespace EvoSC.Modules.Official.MotdModule.Controllers;

[Controller]
public class MotdEditManialinkController(IMotdService motdService) : ManialinkController
{
    [ManialinkRoute(Permission = MotdPermissions.EditMotd)]
    public async Task SaveAsync(EditMotdEntryModel input)
    {
        motdService.SetLocalMotd(input.Text, Context.Player);
        await HideAsync(Context.Player, "MotdModule.MotdEdit");
    }
}
