using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.ServerManagementModule.Events;
using EvoSC.Modules.Official.ServerManagementModule.Interfaces;
using EvoSC.Modules.Official.ServerManagementModule.Permissions;

namespace EvoSC.Modules.Official.ServerManagementModule.Controllers;

[Controller]
public class ServerCommandsController(IServerManagementService serverManagementService) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("setpassword", "Set the player and spectator password for joining the server.",
        ServerManagementPermissions.SetPassword)]
    [CommandAlias("/setpw", true)]
    public async Task SetServerPasswordAsync(string password)
    {
        await serverManagementService.SetPasswordAsync(password);

        Context.AuditEvent
            .WithEventName(ServerManagementAuditEvents.PasswordSet)
            .HavingProperties(new { password })
            .Success();

        await Context.Server.SuccessMessageAsync(Context.Player, "The password was changed.");
    }

    [ChatCommand("clearpassword", "Clear the server password.",
        ServerManagementPermissions.SetPassword)]
    [CommandAlias("/clearpw", true)]
    public async Task ClearServerPasswordAsync()
    {
        await serverManagementService.SetPasswordAsync("");

        Context.AuditEvent
            .WithEventName(ServerManagementAuditEvents.PasswordSet)
            .HavingProperties(new { password = "", cleared = true })
            .Success();
        
        await Context.Server.SuccessMessageAsync(Context.Player, "The password was cleared and removed.");
    }
    
    [ChatCommand("setmaxplayers", "Set maximum number of players that can join the server.",
        ServerManagementPermissions.SetMaxSlots)]
    [CommandAlias("/maxplayers", true)]
    public async Task SetMaxPlayersAsync(int maxPlayers)
    {
        await serverManagementService.SetMaxPlayersAsync(maxPlayers);
        
        Context.AuditEvent
            .WithEventName(ServerManagementAuditEvents.MaxPlayersSet)
            .HavingProperties(new { maxPlayers })
            .Success();
        
        await Context.Server.SuccessMessageAsync(Context.Player, $"Max players set to {maxPlayers}");
    }
    
    [ChatCommand("setmaxspectators", "Set the maximum number of spectators that can join the server.",
        ServerManagementPermissions.SetMaxSlots)]
    [CommandAlias("/maxspectators", true)]
    public async Task SetMaxSpectatorsAsync(int maxSpectators)
    {
        await serverManagementService.SetMaxSpectatorsAsync(maxSpectators);
        
        Context.AuditEvent
            .WithEventName(ServerManagementAuditEvents.MaxSpectatorsSet)
            .HavingProperties(new { maxSpectators })
            .Success();
        
        await Context.Server.SuccessMessageAsync(Context.Player, $"Max spectators set to {maxSpectators}");
    }
}
