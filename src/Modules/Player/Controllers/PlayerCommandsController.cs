using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.Player.Events;
using EvoSC.Modules.Official.Player.Interfaces;

namespace EvoSC.Modules.Official.Player.Controllers;

[Controller]
public class PlayerCommandsController(IPlayerService players) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("kick", "[Command.Kick]", ModPermissions.KickPlayer)]
    public Task KickPlayerAsync(IOnlinePlayer player) => players.KickAsync(player, Context.Player);

    [ChatCommand("mute", "[Command.Mute]", ModPermissions.MutePlayer)]
    public Task MutePlayerAsync(IOnlinePlayer player) => players.MuteAsync(player, Context.Player);
    
    [ChatCommand("unmute", "[Command.Unmute]", ModPermissions.MutePlayer)]
    public Task UnMutePlayerAsync(IOnlinePlayer player) => players.UnmuteAsync(player, Context.Player);
    
    [ChatCommand("ban", "[Command.Ban]", ModPermissions.BanPlayer)]
    public Task BanPlayerAsync(IOnlinePlayer player) => players.BanAsync(player, Context.Player);
    
    [ChatCommand("unban", "[Command.Unban]", ModPermissions.BanPlayer)]
    public Task UnbanPlayerAsync(string login) => players.UnbanAsync(login, Context.Player);

    [ChatCommand("/forcespectator", "Force a player to spectator")]
    [CommandAlias("/forcespec", hide: true)]
    public async Task ForceSpectatorAsync(IOnlinePlayer player)
    {
        await players.ForceSpectatorAsync(player);

        Context.AuditEvent.Success().WithEventName(AuditEvents.PlayerForcedToSpectator);
        await Context.Chat.SuccessMessageAsync($"$<{player.NickName}$> was forced to spectator.", Context.Player);
    }
}
