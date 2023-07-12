using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.Maps.Events;
using EvoSC.Modules.Official.Maps.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Maps.Controllers;

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

    [ChatCommand("add", "[Commmand.Add]")]
    public async Task AddMap(string mapId)
    {
        IMap? map;
        try
        {
            map = await _mxMapService.FindAndDownloadMapAsync(Convert.ToInt32(mapId), null, Context.Player);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, "Failed adding map with ID {MapId}", mapId);
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.FailedAddingMap(mapId), Context.Player);
            return;
        }

        if (map == null)
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.MapIdNotFound(mapId), Context.Player);
            return;
        }

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.MapAdded)
            .HavingProperties(new {Map = map})
            .Comment(_locale.Audit_MapAdded);
        
        await _server.SuccessMessageAsync(_locale.PlayerLanguage.MapAddedSuccessfully(map.Name, map.Author.NickName), Context.Player);
    }

    [ChatCommand("remove", "[Command.Remove]")]
    public async Task RemoveMap(long mapId)
    {
        var map = await _mapService.GetMapByIdAsync(mapId);

        if (map == null)
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.MapIdNotFound(mapId), Context.Player);
            return;
        }

        await _mapService.RemoveMapAsync(mapId);

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.MapRemoved)
            .HavingProperties(new {Map = map})
            .Comment(_locale.Audit_MapRemoved);
        
        await _server.SuccessMessageAsync(_locale.PlayerLanguage.MapRemovedSuccessfully(map.Name, map.Author.NickName), Context.Player);
        _logger.LogInformation("Player {PlayerId} removed map {MapName}", Context.Player.Id, map.Name);
    }
}
