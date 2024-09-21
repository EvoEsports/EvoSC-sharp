using System.Data;
using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.MapsModule.Events;
using EvoSC.Modules.Official.MapsModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MapsModule.Controllers;

[Controller]
public class MapsController(
    ILogger<MapsController> logger,
    IMxMapService mxMapService,
    IMapService mapService,
    Locale locale)
    : EvoScController<ICommandInteractionContext>
{
    private readonly dynamic _locale = locale;

    [ChatCommand("addmap", "[Commmand.Add]", MapsPermissions.AddMap)]
    [CommandAlias("/am", true)]
    public async Task AddMapAsync(string mapId)
    {
        IMap? map;
        try
        {
            map = await mxMapService.FindAndDownloadMapAsync(Convert.ToInt32(mapId), null, Context.Player);
        }
        catch (DuplicateNameException)
        {
            await Context.Chat.ErrorMessageAsync(_locale.PlayerLanguage.DuplicateMap(mapId), Context.Player);
            return;
        }
        catch (Exception)
        {
            await Context.Chat.ErrorMessageAsync(_locale.PlayerLanguage.FailedAddingMap(mapId), Context.Player);
            throw;
        }

        if (map == null)
        {
            await Context.Chat.ErrorMessageAsync(_locale.PlayerLanguage.MapIdNotFound(mapId), Context.Player);
            return;
        }

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.MapAdded)
            .HavingProperties(new { Map = map })
            .Comment(_locale.Audit_MapAdded);

        await Context.Chat.SuccessMessageAsync(_locale.PlayerLanguage.MapAddedSuccessfully(map.Name, map.Author?.NickName), Context.Player);
    }

    [ChatCommand("removemap", "[Command.Remove]", MapsPermissions.RemoveMap)]
    [CommandAlias("/rm", true)]
    public async Task RemoveMapAsync(long mapId)
    {
        var map = await mapService.GetMapByIdAsync(mapId);

        if (map == null)
        {
            await Context.Chat.ErrorMessageAsync(_locale.PlayerLanguage.MapIdNotFound(mapId), Context.Player);
            return;
        }

        try
        {
            await mapService.RemoveMapAsync(mapId);
        }
        catch (Exception)
        {
            await Context.Chat.ErrorMessageAsync(_locale.PlayerLanguage.MapRemovedFailed(mapId), Context.Player);
            throw;
        }

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.MapRemoved)
            .HavingProperties(new { Map = map })
            .Comment(_locale.Audit_MapRemoved);

        await Context.Chat.SuccessMessageAsync(_locale.PlayerLanguage.MapRemovedSuccessfully(map.Name, map.Author.NickName), Context.Player);
        logger.LogDebug("Player {PlayerId} removed map {MapName}", Context.Player.Id, map.Name);
    }
}
