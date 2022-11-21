using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.Player.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Player.Controllers;

[Controller]
public class PlayerCommandsController : EvoScController<CommandInteractionContext>
{
    private readonly IPlayerService _players;

    public PlayerCommandsController(IPlayerService players) => _players = players;

    [ChatCommand("kick", "Kick a player from the server.", ModPermissions.KickPlayer)]
    public Task GetPlayersAsync(IOnlinePlayer player) => _players.KickAsync(player);
}
