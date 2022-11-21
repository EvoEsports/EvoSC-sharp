using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Player.Controllers;

[Controller]
public class PlayerCommandsController : EvoScController<CommandInteractionContext>
{
    private readonly ILogger<PlayerCommandsController> _logger;
    private readonly IPlayerManagerService _playerManager;
    
    public PlayerCommandsController(ILogger<PlayerCommandsController> logger, IPlayerManagerService playerManager)
    {
        _logger = logger;
        _playerManager = playerManager;
    }

    [ChatCommand("kick", "Kick a player.", ModPermissions.KickPlayer)]
    public async Task GetPlayersAsync(string searchPattern)
    {
        _logger.LogInformation("Searching for: {pattern}", searchPattern);

        var players = await _playerManager.FindOnlinePlayerAsync(searchPattern);

        foreach (var player in players)
        {
            _logger.LogDebug("Found player: {name}", player.NickName);
        }
    }
}
