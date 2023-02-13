using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.Maps.Events;
using EvoSC.Modules.Official.Maps.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Maps.Controllers;

[Controller]
public class MapsController : EvoScController<CommandInteractionContext>
{
    private readonly ILogger<MapsController> _logger;
    private readonly IMxMapService _mxMapService;
    private readonly IMapService _mapService;
    private readonly IServerClient _server;

    public MapsController(ILogger<MapsController> logger, IMxMapService mxMapService, IMapService mapService,
        IServerClient server)
    {
        _logger = logger;
        _mxMapService = mxMapService;
        _mapService = mapService;
        _server = server;
    }

    [ChatCommand("add", "Adds a map to the server")]
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
            await _server.ErrorMessageAsync($"Something went wrong while trying to add map with ID {mapId}.");
            return;
        }

        if (map == null)
        {
            await _server.WarningMessageAsync($"Map with ID {mapId} could not be found.");
            return;
        }

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.MapAdded)
            .HavingProperties(new {Map = map})
            .Comment("Map was added.");
        
        await _server.SuccessMessageAsync($"Added {map.Name} by {map.Author.NickName} to the server.");
    }

    [ChatCommand("remove", "Removes a map from the server")]
    public async Task RemoveMap(long mapId)
    {
        var map = await _mapService.GetMapByIdAsync(mapId);

        if (map == null)
        {
            await _server.WarningMessageAsync($"Map with ID {mapId} could not be found.");
            return;
        }

        await _mapService.RemoveMapAsync(mapId);

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.MapRemoved)
            .HavingProperties(new {Map = map})
            .Comment("Map was removed.");
        
        await _server.SuccessMessageAsync($"Removed map with ID {mapId} from the maplist.");
        _logger.LogInformation("Player {PlayerId} removed map {MapName}", Context.Player.Id, map.Name);
    }
}
