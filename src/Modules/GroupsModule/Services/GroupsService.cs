using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Permissions.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Validation;
using EvoSC.Modules.Official.GroupsModule.Interfaces;
using EvoSC.Modules.Official.GroupsModule.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.GroupsModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class GroupsService(
    IGroupsRepository repository,
    IPermissionRepository permissionRepository,
    IPermissionManager permissions,
    IPlayerManagerService players,
    IPlayerCacheService playerCache,
    IManialinkManager manialinks,
    IServerClient server,
    ILogger<GroupsService> logger) : IGroupsService
{
    public const int PlayersPerPage = 10;
    public const string DefaultColor = "FFF";

    /// <summary>
    /// The groups every server starts with, which are relied on and can't be deleted.
    /// </summary>
    private const int LastBuiltInGroupId = 2;

    private const string ModuleName = "GroupsModule";
    private const string GroupListName = $"{ModuleName}.GroupList";
    private const string GroupEditName = $"{ModuleName}.GroupEdit";
    private const string GroupPermissionsName = $"{ModuleName}.GroupPermissions";
    private const string PlayerListName = $"{ModuleName}.PlayerList";
    private const string PlayerGroupsName = $"{ModuleName}.PlayerGroups";
    private const string DeleteDialogName = $"{ModuleName}.Dialogs.ConfirmDeleteGroupDialog";

    private static readonly string[] AllWindows =
        [GroupListName, GroupEditName, GroupPermissionsName, PlayerListName, PlayerGroupsName];

    public async Task ShowGroupListAsync(IPlayer player)
    {
        var groups = await repository.GetGroupsAsync();
        var memberCounts = await repository.GetMemberCountsAsync();
        var permissionCounts = await repository.GetPermissionCountsAsync();

        var items = groups
            .Select(g => new GroupListItem(
                g.Id,
                g.Title,
                g.Icon ?? "",
                g.Color ?? DefaultColor,
                g.Unrestricted,
                g.Id <= LastBuiltInGroupId,
                memberCounts.GetValueOrDefault(g.Id),
                permissionCounts.GetValueOrDefault(g.Id)))
            .ToList();

        await ShowWindowAsync(player, GroupListName, new { groups = items });
    }

    public async Task ShowGroupEditorAsync(IPlayer player, int? groupId, GroupEditModel? input = null,
        FormValidationResult? validation = null)
    {
        if (input == null)
        {
            if (groupId == null)
            {
                input = new GroupEditModel { Color = DefaultColor };
            }
            else
            {
                var group = await permissions.GetGroupAsync(groupId.Value);

                if (group == null)
                {
                    await server.Chat.ErrorMessageAsync("The group does not exist.", player);
                    return;
                }

                input = new GroupEditModel
                {
                    Title = group.Title,
                    Description = group.Description,
                    Icon = group.Icon,
                    Color = group.Color,
                    Unrestricted = group.Unrestricted
                };
            }
        }

        await ShowWindowAsync(player, GroupEditName, new
        {
            Validation = validation ?? new FormValidationResult(),
            groupId = groupId ?? 0,
            input
        });
    }

    public async Task ShowGroupPermissionsAsync(IPlayer player, int groupId)
    {
        var group = await permissions.GetGroupAsync(groupId);

        if (group == null)
        {
            await server.Chat.ErrorMessageAsync("The group does not exist.", player);
            return;
        }

        var granted = await repository.GetGroupPermissionIdsAsync(groupId);
        var toggles = (await repository.GetPermissionsAsync())
            .Select(p => new PermissionToggle(p.Id, p.Name, DescriptionText(p.Description), granted.Contains(p.Id)))
            .ToList();

        await ShowWindowAsync(player, GroupPermissionsName, new
        {
            groupId,
            groupTitle = group.Title,
            unrestricted = group.Unrestricted,
            permissions = toggles
        });
    }

    public async Task ShowDeleteGroupDialogAsync(IPlayer player, int groupId)
    {
        var group = await permissions.GetGroupAsync(groupId);

        if (group == null)
        {
            await server.Chat.ErrorMessageAsync("The group does not exist.", player);
            return;
        }

        await manialinks.SendManialinkAsync(player, DeleteDialogName, new { groupId, groupTitle = group.Title });
    }

    public async Task ShowPlayerListAsync(IPlayer player, string? search, int page)
    {
        search = string.IsNullOrWhiteSpace(search) ? null : search.Trim();

        var (found, total) = await repository.GetPlayersAsync(search, Math.Max(0, page) * PlayersPerPage,
            PlayersPerPage);
        var pageCount = Math.Max(1, (int)Math.Ceiling(total / (double)PlayersPerPage));

        // the page might no longer exist, e.g. after narrowing the search
        if (page >= pageCount)
        {
            await ShowPlayerListAsync(player, search, pageCount - 1);
            return;
        }

        var groups = (await repository.GetGroupsAsync()).ToDictionary(g => g.Id);
        var groupIds = await repository.GetPlayersGroupIdsAsync(found.Select(p => p.Id));

        var items = found
            .Select(p => new PlayerListItem(
                p.AccountId,
                FormattingUtils.CleanTmFormatting(p.NickName),
                string.Join(", ", groupIds.GetValueOrDefault(p.Id, [])
                    .Where(groups.ContainsKey)
                    .Select(id => groups[id].Title))))
            .ToList();

        await ShowWindowAsync(player, PlayerListName, new
        {
            players = items,
            search = search ?? "",
            page = Math.Max(0, page),
            pageCount,
            total
        });
    }

    public async Task ShowPlayerGroupsAsync(IPlayer player, string accountId, string? search, int page)
    {
        var target = await players.GetPlayerAsync(accountId);

        if (target == null)
        {
            await server.Chat.ErrorMessageAsync("The player does not exist.", player);
            return;
        }

        var memberOf = await repository.GetPlayerGroupIdsAsync(target.Id);
        var toggles = (await repository.GetGroupsAsync())
            .Select(g => new GroupToggle(g.Id, g.Title, g.Icon ?? "", g.Color ?? DefaultColor, g.Unrestricted,
                memberOf.Contains(g.Id)))
            .ToList();

        await ShowWindowAsync(player, PlayerGroupsName, new
        {
            accountId,
            nickName = FormattingUtils.CleanTmFormatting(target.NickName),
            groups = toggles,
            search = search ?? "",
            page
        });
    }

    public async Task<int?> SaveGroupAsync(IPlayer actor, int? groupId, GroupEditModel input)
    {
        var title = input.Title.Trim();
        var description = input.Description?.Trim() ?? "";
        var icon = string.IsNullOrWhiteSpace(input.Icon) ? "" : input.Icon.Trim();
        var color = string.IsNullOrWhiteSpace(input.Color) ? DefaultColor : input.Color.Trim().ToUpperInvariant();

        if (groupId == null)
        {
            var created = await permissionRepository.AddGroupAsync(new Group
            {
                Title = title,
                Description = description,
                Icon = icon,
                Color = color,
                Unrestricted = input.Unrestricted,
                Permissions = []
            });

            await server.Chat.SuccessMessageAsync($"Created the group '{title}'.", actor);
            return created.Id;
        }

        var group = await permissions.GetGroupAsync(groupId.Value);

        if (group == null)
        {
            await server.Chat.ErrorMessageAsync("The group does not exist.", actor);
            return null;
        }

        if (group.Unrestricted && !input.Unrestricted && await IsMemberAsync(actor, group.Id))
        {
            await server.Chat.ErrorMessageAsync(
                "You are in this group, so you can't take its unrestricted permissions away.", actor);
            return null;
        }

        group.Title = title;
        group.Description = description;
        group.Icon = icon;
        group.Color = color;
        group.Unrestricted = input.Unrestricted;

        await permissions.UpdateGroupAsync(group);
        await RefreshOnlinePlayersAsync();

        await server.Chat.SuccessMessageAsync($"Saved the group '{title}'.", actor);
        return group.Id;
    }

    public async Task<bool> DeleteGroupAsync(IPlayer actor, int groupId)
    {
        var group = await permissions.GetGroupAsync(groupId);

        if (group == null)
        {
            await server.Chat.ErrorMessageAsync("The group does not exist.", actor);
            return false;
        }

        if (group.Id <= LastBuiltInGroupId)
        {
            await server.Chat.ErrorMessageAsync($"The group '{group.Title}' is built in and can't be deleted.", actor);
            return false;
        }

        if (group.Unrestricted && await IsMemberAsync(actor, group.Id))
        {
            await server.Chat.ErrorMessageAsync("You are in this group, so you can't delete it.", actor);
            return false;
        }

        await permissions.RemoveGroupAsync(group.Id);
        await RefreshOnlinePlayersAsync();

        await server.Chat.SuccessMessageAsync($"Deleted the group '{group.Title}'.", actor);
        return true;
    }

    public async Task<(string Name, bool Granted)?> TogglePermissionAsync(int groupId, int permissionId)
    {
        var group = await permissions.GetGroupAsync(groupId);
        var permission = (await repository.GetPermissionsAsync()).FirstOrDefault(p => p.Id == permissionId);

        if (group == null || permission == null)
        {
            return null;
        }

        var granted = (await repository.GetGroupPermissionIdsAsync(groupId)).Contains(permissionId);

        if (granted)
        {
            await permissions.RemovePermissionFromGroupAsync(group, permission);
        }
        else
        {
            await permissions.AddPermissionToGroupAsync(group, permission);
        }

        return (permission.Name, !granted);
    }

    public async Task<bool?> TogglePlayerGroupAsync(IPlayer actor, string accountId, int groupId)
    {
        var target = await players.GetPlayerAsync(accountId);
        var group = await permissions.GetGroupAsync(groupId);

        if (target == null || group == null)
        {
            await server.Chat.ErrorMessageAsync("The player or group does not exist.", actor);
            return null;
        }

        var isMember = await IsMemberAsync(target, groupId);

        if (!isMember)
        {
            await permissions.AddPlayerToGroupAsync(target, group);
            await NotifyRoleReceivedAsync(target, group);
                return true;
        }

        if (group.Unrestricted && target.Id == actor.Id)
        {
            await server.Chat.ErrorMessageAsync(
                $"You can't remove yourself from the unrestricted group '{group.Title}'.", actor);
            return null;
        }

        await permissions.RemovePlayerFromGroupAsync(target, group);
        return false;
    }

    /// <summary>
    /// Tell a player which group they were just added to. Players who are offline simply miss the message.
    /// </summary>
    private async Task NotifyRoleReceivedAsync(IPlayer player, IGroup group)
    {
        try
        {
            await server.Chat.InfoMessageAsync(
                $"You received the role {FormattingUtils.CleanTmFormatting(group.Title)}.", player);
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Failed to notify player {AccountId} about their new role", player.AccountId);
        }
    }

    private async Task<bool> IsMemberAsync(IPlayer player, int groupId) =>
        (await repository.GetPlayerGroupIdsAsync(player.Id)).Contains(groupId);

    /// <summary>
    /// Make online players pick up changes to their groups.
    /// </summary>
    private async Task RefreshOnlinePlayersAsync()
    {
        try
        {
            foreach (var online in await players.GetOnlinePlayersAsync())
            {
                await playerCache.InvalidatePlayerStateAsync(online);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to refresh online players after a group change");
        }
    }

    /// <summary>
    /// Show one of the windows, closing the others so they don't pile up.
    /// </summary>
    private async Task ShowWindowAsync(IPlayer player, string name, object data)
    {
        foreach (var other in AllWindows.Where(w => w != name))
        {
            await manialinks.HideManialinkAsync(player, other);
        }

        await manialinks.SendManialinkAsync(player, name, data);
    }

    /// <summary>
    /// Descriptions can be a key to a translation, which there is nothing to look up for here.
    /// </summary>
    private static string DescriptionText(string description) =>
        description.StartsWith('[') ? "" : description;
}
