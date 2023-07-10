using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.Player.Interfaces;

namespace EvoSC.Modules.Official.Player.Controllers;

[Controller]
public class PlayerCommandsController : EvoScController<ICommandInteractionContext>
{
    private readonly IPlayerService _players;

    public PlayerCommandsController(IPlayerService players) => _players = players;

    [ChatCommand("kick", "[Command.Kick]", ModPermissions.KickPlayer)]
    public Task KickPlayerAsync(IOnlinePlayer player) => _players.KickAsync(player, Context.Player);

    [ChatCommand("mute", "[Command.Mute]", ModPermissions.MutePlayer)]
    public Task MutePlayerAsync(IOnlinePlayer player) => _players.MuteAsync(player, Context.Player);
    
    [ChatCommand("unmute", "[Command.Unmute]", ModPermissions.MutePlayer)]
    public Task UnMutePlayerAsync(IOnlinePlayer player) => _players.UnmuteAsync(player, Context.Player);
    
    [ChatCommand("ban", "[Command.Ban]", ModPermissions.BanPlayer)]
    public Task BanPlayerAsync(IOnlinePlayer player) => _players.BanAsync(player, Context.Player);
    
    [ChatCommand("unban", "[Command.Unban]", ModPermissions.BanPlayer)]
    public Task UnbanPlayerAsync(string login) => _players.UnbanAsync(login, Context.Player);
}
