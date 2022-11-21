using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
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

    [ChatCommand("players", "Get a list of players.")]
    public async Task GetPlayersAsync()
    {
        foreach (var player in await _playerManager.GetOnlinePlayersAsync())
        {
            _logger.LogInformation("Found player: {Name}", player.NickName);
        }
    }
}
