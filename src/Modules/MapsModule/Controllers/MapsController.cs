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
public class MapsController : EvoScController<ICommandInteractionContext>
{
    private readonly ILogger<MapsController> _logger;
    private readonly IMxMapService _mxMapService;
    private readonly IMapService _mapService;
    private readonly IServerClient _server;
    private readonly dynamic _locale;

    public MapsController(ILogger<MapsController> logger, IMxMapService mxMapService, IMapService mapService,
        IServerClient server, Locale locale)
    {
        _logger = logger;
        _mxMapService = mxMapService;
        _mapService = mapService;
        _server = server;
        _locale = locale;
    }

    [ChatCommand("add", "[Commmand.Add]", MapsPermissions.AddMap)]
    public async Task AddMapAsync(string mapId)
    {
        IMap? map;
        try
        {
            map = await _mxMapService.FindAndDownloadMapAsync(Convert.ToInt32(mapId), null, Context.Player);
        }
        catch (DuplicateNameException)
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.DuplicateMap(mapId), Context.Player);
            return;
        }
        catch (Exception)
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.FailedAddingMap(mapId), Context.Player);
            throw;
        }

        if (map == null)
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.MapIdNotFound(mapId), Context.Player);
            return;
        }

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.MapAdded)
            .HavingProperties(new { Map = map })
            .Comment(_locale.Audit_MapAdded);

        await _server.SuccessMessageAsync(_locale.PlayerLanguage.MapAddedSuccessfully(map.Name, map.Author?.NickName),
            Context.Player);
    }

    [ChatCommand("remove", "[Command.Remove]", MapsPermissions.RemoveMap)]
    public async Task RemoveMapAsync(long mapId)
    {
        var map = await _mapService.GetMapByIdAsync(mapId);

        if (map == null)
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.MapIdNotFound(mapId), Context.Player);
            return;
        }

        try
        {
            await _mapService.RemoveMapAsync(mapId);
        }
        catch (Exception)
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.MapRemovedFailed(mapId), Context.Player);
            throw;
        }

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.MapRemoved)
            .HavingProperties(new { Map = map })
            .Comment(_locale.Audit_MapRemoved);

        await _server.SuccessMessageAsync(_locale.PlayerLanguage.MapRemovedSuccessfully(map.Name, map.Author.NickName),
            Context.Player);
        _logger.LogDebug("Player {PlayerId} removed map {MapName}", Context.Player.Id, map.Name);
    }
}
