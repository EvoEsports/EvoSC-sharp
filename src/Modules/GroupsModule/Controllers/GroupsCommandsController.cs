using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.GroupsModule.Interfaces;
using EvoSC.Modules.Official.GroupsModule.Permissions;

namespace EvoSC.Modules.Official.GroupsModule.Controllers;

[Controller]
public class GroupsCommandsController(IGroupsService groupsService) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("groups", "Manage groups and their permissions.", GroupsPermissions.ManageGroups)]
    public async Task ShowGroupsAsync()
    {
        await groupsService.ShowGroupListAsync(Context.Player);
        Context.AuditEvent.Cancel();
    }

    [ChatCommand("roles", "Manage the groups of players.", GroupsPermissions.ManagePlayerGroups)]
    public async Task ShowPlayerRolesAsync()
    {
        await groupsService.ShowPlayerListAsync(Context.Player, null, 0);
        Context.AuditEvent.Cancel();
    }
}
