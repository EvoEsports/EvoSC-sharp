using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.ASayModule.Interfaces;

namespace EvoSC.Modules.Official.ASayModule.Controllers;

[Controller]
public class ASayController(IASayService asayService) : EvoScController<CommandInteractionContext>
{
    [ChatCommand("asay", "Shows a message to all connected players as manialink.", ASayPermissions.UseASay, true)]
    public async Task ShowAnnounceMessageToPlayersAsync(string? text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            await asayService.ShowAnnouncementAsync(text);

        }
        else
        {
            await asayService.HideAnnouncementAsync();
        }
    }
    
    [ChatCommand("clearasay", "Hides the announcement message from all connected players.", ASayPermissions.UseASay, true)]
    public async Task ClearAnnouncementMessageForPlayersAsync()
    {
        await asayService.HideAnnouncementAsync();
    }
}
