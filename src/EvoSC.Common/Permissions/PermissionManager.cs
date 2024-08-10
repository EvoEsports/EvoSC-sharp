using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;

namespace EvoSC.Common.Permissions;

public class PermissionManager(IPermissionRepository permissionRepository, IPlayerCacheService playerCache) : IPermissionManager
{
    public async Task<bool> HasPermissionAsync(IPlayer player, string permission)
    {
        var result = await permissionRepository.GetPlayerPermissionsAsync(player.Id);

        var permNames = result.Select(p => p.Name);

        foreach (var permName in permNames)
        {
            if (permName.Equals(permission, StringComparison.Ordinal))
            {
                return true;
            }
        }

        var groups = await permissionRepository.GetGroupsAsync(player.Id);

        foreach (var group in groups)
        {
            if (group.Unrestricted)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<IPermission?> GetPermissionAsync(string name) => await permissionRepository.GetPermissionAsync(name);

    public Task AddPermissionAsync(IPermission permission) => permissionRepository.AddPermissionAsync(permission);

    public Task UpdatePermissionAsync(IPermission permission) => permissionRepository.UpdatePermissionAsync(permission);

    public async Task RemovePermissionAsync(string name)
    {
        var permission = await permissionRepository.GetPermissionAsync(name);

        if (permission == null)
        {
            throw new InvalidOperationException($"Permission with name '{name}' does not exist.");
        }

        await permissionRepository.RemovePermissionAsync(permission);
    }

    public Task RemovePermissionAsync(IPermission permission) => RemovePermissionAsync(permission.Name);

    public Task AddGroupAsync(IGroup group) => permissionRepository.AddGroupAsync(group);

    public async Task RemoveGroupAsync(int id)
    {
        var group = await permissionRepository.GetGroupAsync(id);

        if (group == null)
        {
            throw new InvalidOperationException($"Group with id {id} does not exist.");
        }

        await permissionRepository.RemoveGroupAsync(group);
    }

    public Task RemoveGroupAsync(IGroup group) => RemoveGroupAsync(group.Id);

    public Task UpdateGroupAsync(IGroup group) => permissionRepository.UpdateGroupAsync(group);

    public async Task<IGroup?> GetGroupAsync(int id) => await permissionRepository.GetGroupAsync(id);

    public Task AddPlayerToGroupAsync(IPlayer player, IGroup group) => AddPlayerToGroupAsync(player, group.Id);

    public async Task AddPlayerToGroupAsync(IPlayer player, int group)
    {
        await permissionRepository.AddPlayerToGroupAsync(player.Id, group);
        await playerCache.InvalidatePlayerStateAsync(player);
    }

    public async Task RemovePlayerFromGroupAsync(IPlayer player, IGroup group)
    {
        await permissionRepository.RemovePlayerFromGroupAsync(player.Id, group.Id);
        await playerCache.InvalidatePlayerStateAsync(player);
    }

    public Task SetDisplayGroupAsync(IPlayer player, IGroup group) => SetDisplayGroupAsync(player, group.Id);
    
    public async Task SetDisplayGroupAsync(IPlayer player, int groupId)
    {
        await permissionRepository.SetDisplayGroupAsync(player.Id, groupId);
        await playerCache.InvalidatePlayerStateAsync(player);
    }

    public async Task AddPermissionToGroupAsync(IGroup group, IPermission permission) =>
        await permissionRepository.AddPermissionToGroupAsync(group.Id, permission.Id);

    public async Task RemovePermissionFromGroupAsync(IGroup group, IPermission permission) =>
        await permissionRepository.RemovePermissionFromGroupAsync(group.Id, permission.Id);

    public async Task ClearGroupPermissionsAsync(IGroup group) =>
        await permissionRepository.ClearGroupPermissionsAsync(group.Id);
}
