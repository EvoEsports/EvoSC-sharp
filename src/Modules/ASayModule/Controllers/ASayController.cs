using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ASayModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ASayModule.Controllers;

[Controller]
public class ASayController : EvoScController<CommandInteractionContext>
{
    private readonly ILogger<ASayController> _logger;
    private readonly IASayService _asayService;

    // You want to dependency inject the needed services here at the constructor
    public ASayController(ILogger<ASayController> logger, IASayService asayService)
    {
        _logger = logger;
        _asayService = asayService;
    }

    [ChatCommand("asay", "Shows a message to all connected players as manialink.", ASayPermissions.UseASay, true)]
    public async Task ShowAnnounceMessageToPlayers(string? text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            await _asayService.ShowAnnouncement(text);
        }
        else
        {
            await _asayService.HideAnnouncement();
        }
    }
    
    [ChatCommand("clearasay", "Hides the announcement message from all connected players.", ASayPermissions.UseASay, true)]
    public async Task ClearAnnouncementMessageToPlayers()
    {
        await _asayService.HideAnnouncement();
    }
}
