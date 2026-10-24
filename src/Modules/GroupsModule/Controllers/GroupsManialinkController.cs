using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.Official.GroupsModule.Events;
using EvoSC.Modules.Official.GroupsModule.Interfaces;
using EvoSC.Modules.Official.GroupsModule.Models;
using EvoSC.Modules.Official.GroupsModule.Permissions;

namespace EvoSC.Modules.Official.GroupsModule.Controllers;

[Controller]
[ManialinkRoute(Route = "Groups", Permission = GroupsPermissions.ManageGroups)]
public class GroupsManialinkController(IGroupsService groupsService) : ManialinkController
{
    public async Task ShowGroupListAsync()
    {
        await groupsService.ShowGroupListAsync(Context.Player);
        Context.AuditEvent.Cancel();
    }

    public async Task NewGroupAsync()
    {
        await groupsService.ShowGroupEditorAsync(Context.Player, null);
        Context.AuditEvent.Cancel();
    }

    public async Task EditGroupAsync(int groupId)
    {
        await groupsService.ShowGroupEditorAsync(Context.Player, groupId);
        Context.AuditEvent.Cancel();
    }

    /// <summary>
    /// Create a group if the ID is 0, otherwise update the group with the ID.
    /// </summary>
    public async Task SaveGroupAsync(int groupId, GroupEditModel input)
    {
        int? id = groupId == 0 ? null : groupId;

        if (!IsModelValid)
        {
            await groupsService.ShowGroupEditorAsync(Context.Player, id, input, ModelValidation);
            Context.AuditEvent.Cancel();
            return;
        }

        var savedId = await groupsService.SaveGroupAsync(Context.Player, id, input);

        if (savedId == null)
        {
            Context.AuditEvent.Cancel();
            return;
        }

        Context.AuditEvent
            .WithEventName(id == null ? GroupsAuditEvents.GroupCreated : GroupsAuditEvents.GroupUpdated)
            .HavingProperties(new { GroupId = savedId, input })
            .Success();

        // a new group has no permissions, so continue with giving it some
        if (id == null)
        {
            await groupsService.ShowGroupPermissionsAsync(Context.Player, savedId.Value);
        }
        else
        {
            await groupsService.ShowGroupListAsync(Context.Player);
        }
    }

    public async Task ShowGroupPermissionsAsync(int groupId)
    {
        await groupsService.ShowGroupPermissionsAsync(Context.Player, groupId);
        Context.AuditEvent.Cancel();
    }

    public async Task ToggleGroupPermissionAsync(int groupId, int permissionId)
    {
        var result = await groupsService.TogglePermissionAsync(groupId, permissionId);

        if (result == null)
        {
            Context.AuditEvent.Cancel();
            return;
        }

        var (name, granted) = result.Value;

        Context.AuditEvent
            .WithEventName(granted ? GroupsAuditEvents.GroupPermissionAdded : GroupsAuditEvents.GroupPermissionRemoved)
            .HavingProperties(new { GroupId = groupId, Permission = name })
            .Success();

        await groupsService.ShowGroupPermissionsAsync(Context.Player, groupId);
    }

    public async Task DeleteGroupAsync(int groupId)
    {
        await groupsService.ShowDeleteGroupDialogAsync(Context.Player, groupId);
        Context.AuditEvent.Cancel();
    }

    public async Task ConfirmDeleteGroupAsync(int groupId, bool confirmed)
    {
        await HideAsync(Context.Player, "GroupsModule.Dialogs.ConfirmDeleteGroupDialog");

        if (!confirmed || !await groupsService.DeleteGroupAsync(Context.Player, groupId))
        {
            Context.AuditEvent.Cancel();
            return;
        }

        Context.AuditEvent
            .WithEventName(GroupsAuditEvents.GroupDeleted)
            .HavingProperties(new { GroupId = groupId })
            .Success();

        await groupsService.ShowGroupListAsync(Context.Player);
    }
}
