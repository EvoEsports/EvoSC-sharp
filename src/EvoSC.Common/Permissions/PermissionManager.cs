using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;

namespace EvoSC.Common.Permissions;

public class PermissionManager : IPermissionManager
{
    private readonly IPermissionRepository _permissionRepository;

    public PermissionManager(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<bool> HasPermissionAsync(IPlayer player, string permission)
    {
        var result = await _permissionRepository.GetPlayerPermissionsAsync(player.Id);

        var permNames = result.Select(p => p.Name);

        foreach (var permName in permNames)
        {
            if (permName.Equals(permission, StringComparison.Ordinal))
            {
                return true;
            }
        }

        var groups = await _permissionRepository.GetGroupsAsync(player.Id);

        foreach (var group in groups)
        {
            if (group.Unrestricted)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<IPermission?> GetPermissionAsync(string name) => await _permissionRepository.GetPermissionAsync(name);

    public Task AddPermissionAsync(IPermission permission) => _permissionRepository.AddPermissionAsync(permission);

    public Task UpdatePermissionAsync(IPermission permission) => _permissionRepository.UpdatePermissionAsync(permission);

    public async Task RemovePermissionAsync(string name)
    {
        var permission = await _permissionRepository.GetPermissionAsync(name);

        if (permission == null)
        {
            throw new InvalidOperationException($"Permission with name '{name}' does not exist.");
        }

        await _permissionRepository.RemovePermissionAsync(permission);
    }

    public Task RemovePermissionAsync(IPermission permission) => RemovePermissionAsync(permission.Name);

    public Task AddGroupAsync(IGroup group) => _permissionRepository.AddGroupAsync(group);

    public async Task RemoveGroupAsync(int id)
    {
        var group = await _permissionRepository.GetGroupAsync(id);

        if (group == null)
        {
            throw new InvalidOperationException($"Group with id {id} does not exist.");
        }

        await _permissionRepository.RemoveGroupAsync(group);
    }

    public Task RemoveGroupAsync(IGroup group) => RemoveGroupAsync(group.Id);

    public Task UpdateGroupAsync(IGroup group) => _permissionRepository.UpdateGroupAsync(group);

    public async Task<IGroup?> GetGroupAsync(int id) => await _permissionRepository.GetGroupAsync(id);

    public async Task AddPlayerToGroupAsync(IPlayer player, IGroup group) =>
        await _permissionRepository.AddPlayerToGroupAsync(player.Id, group.Id);

    public async Task RemovePlayerFromGroupAsync(IPlayer player, IGroup group) =>
        await _permissionRepository.RemovePlayerFromGroupAsync(player.Id, group.Id);

    public async Task AddPermissionToGroupAsync(IGroup group, IPermission permission) =>
        await _permissionRepository.AddPermissionToGroupAsync(group.Id, permission.Id);

    public async Task RemovePermissionFromGroupAsync(IGroup group, IPermission permission) =>
        await _permissionRepository.RemovePermissionFromGroupAsync(group.Id, permission.Id);

    public async Task ClearGroupPermissionsAsync(IGroup group) =>
        await _permissionRepository.ClearGroupPermissionsAsync(group.Id);
}
