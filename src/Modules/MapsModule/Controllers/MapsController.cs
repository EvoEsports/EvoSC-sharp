using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models;
using EvoSC.Common.Util;
using EvoSC.Common.Util.ServerUtils;
using EvoSC.Modules.Official.Maps.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Maps.Controllers;

[Controller]
public class MapsController : EvoScController<CommandInteractionContext>
{
    private readonly ILogger<MapsController> _logger;
    private readonly IMxMapService _mxMapService;
    private readonly IServerClient _server;

    public MapsController(ILogger<MapsController> logger, IMxMapService mxMapService, IServerClient server)
    {
        _logger = logger;
        _mxMapService = mxMapService;
        _server = server;
    }

    [ChatCommand("add", "Adds a map to the server")]
    public async Task AddMap(int mapId)
    {
        IMap? map;
        try
        {
            map = await _mxMapService.FindAndDownloadMap(mapId, null, Context.Player);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"Failed adding map with ID {mapId}.");
            await _server.SendChatMessage($"Error: Something went wrong while trying to add map with ID {mapId}.");
            return;
        }

        if (map == null)
        {
            await _server.SendChatMessage($"Map with ID {mapId} could not be found.");
            return;
        }

        await _server.SendChatMessage($"Added {map.Name} by {map.Author.NickName} to the server.");
    }
}
