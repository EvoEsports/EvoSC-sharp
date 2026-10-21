using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.Official.GroupsModule.Events;
using EvoSC.Modules.Official.GroupsModule.Interfaces;
using EvoSC.Modules.Official.GroupsModule.Models;
using EvoSC.Modules.Official.GroupsModule.Permissions;

namespace EvoSC.Modules.Official.GroupsModule.Controllers;

[Controller]
[ManialinkRoute(Route = "PlayerRoles", Permission = GroupsPermissions.ManagePlayerGroups)]
public class PlayerRolesManialinkController(IGroupsService groupsService) : ManialinkController
{
    /// <summary>
    /// Show the first page of the players, filtered by the search entry if there is one.
    /// </summary>
    public async Task ShowPlayerListAsync(PlayerSearchModel input)
    {
        await groupsService.ShowPlayerListAsync(Context.Player, input.Search, 0);
        Context.AuditEvent.Cancel();
    }

    public async Task ShowPlayerListPageAsync(int page, PlayerSearchModel input)
    {
        await groupsService.ShowPlayerListAsync(Context.Player, input.Search, page);
        Context.AuditEvent.Cancel();
    }

    public async Task EditPlayerGroupsAsync(string accountId, int page, PlayerSearchModel input)
    {
        await groupsService.ShowPlayerGroupsAsync(Context.Player, accountId, input.Search, page);
        Context.AuditEvent.Cancel();
    }

    public async Task TogglePlayerGroupAsync(string accountId, int groupId, int page, PlayerSearchModel input)
    {
        var isMember = await groupsService.TogglePlayerGroupAsync(Context.Player, accountId, groupId);

        if (isMember == null)
        {
            Context.AuditEvent.Cancel();
            return;
        }

        Context.AuditEvent
            .WithEventName(isMember.Value ? GroupsAuditEvents.PlayerGroupAdded : GroupsAuditEvents.PlayerGroupRemoved)
            .HavingProperties(new { AccountId = accountId, GroupId = groupId })
            .Success();

        await groupsService.ShowPlayerGroupsAsync(Context.Player, accountId, input.Search, page);
    }
}
