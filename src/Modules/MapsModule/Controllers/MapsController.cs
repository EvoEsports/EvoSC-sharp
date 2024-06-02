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
        IServerClient server,
        Locale locale
    )
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
            await server.ErrorMessageAsync(Context.Player, _locale.PlayerLanguage.DuplicateMap(mapId));
            return;
        }
        catch (Exception)
        {
            await server.ErrorMessageAsync(Context.Player, _locale.PlayerLanguage.FailedAddingMap(mapId));
            throw;
        }

        if (map == null)
        {
            await server.ErrorMessageAsync(Context.Player, _locale.PlayerLanguage.MapIdNotFound(mapId));
            return;
        }

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.MapAdded)
            .HavingProperties(new { Map = map })
            .Comment(_locale.Audit_MapAdded);

        await server.SuccessMessageAsync(Context.Player, _locale.PlayerLanguage.MapAddedSuccessfully(map.Name, map.Author?.NickName));
    }

    [ChatCommand("removemap", "[Command.Remove]", MapsPermissions.RemoveMap)]
    [CommandAlias("/rm", true)]
    public async Task RemoveMapAsync(long mapId)
    {
        var map = await mapService.GetMapByIdAsync(mapId);

        if (map == null)
        {
            await server.ErrorMessageAsync(Context.Player, _locale.PlayerLanguage.MapIdNotFound(mapId));
            return;
        }

        try
        {
            await mapService.RemoveMapAsync(mapId);
        }
        catch (Exception)
        {
            await server.ErrorMessageAsync(Context.Player, _locale.PlayerLanguage.MapRemovedFailed(mapId));
            throw;
        }

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.MapRemoved)
            .HavingProperties(new { Map = map })
            .Comment(_locale.Audit_MapRemoved);

        await server.SuccessMessageAsync(Context.Player, _locale.PlayerLanguage.MapRemovedSuccessfully(map.Name, map.Author.NickName));
        logger.LogDebug("Player {PlayerId} removed map {MapName}", Context.Player.Id, map.Name);
    }
}
