using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Repository;
using EvoSC.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;
using RepoDb.Extensions;

namespace EvoSC.Common.Permissions;

public class PermissionManager : IPermissionManager
{
    private readonly ILogger<PermissionManager> _logger;
    private readonly IPermissionRepository _permissionRepository;

    public PermissionManager(ILogger<PermissionManager> logger, IPermissionRepository permissionRepository)
    {
        _logger = logger;
        _permissionRepository = permissionRepository;
    }

    public async Task<bool> HasPermissionAsync(IPlayer player, string permission)
    {
        var result = await _permissionRepository.GetPlayerPermissionsAsync(player.Id);

        var permissions = result.ToList();
        if (permissions.IsNullOrEmpty())
        {
            _logger.LogDebug("Player {Id} does not have permission {PermName}.", player.Id, permission);
            return false;
        }

        var permNames = permissions.Select(p => p.Name);

        foreach (var permName in permNames)
        {
            if (permName.Equals(permission))
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

    public async Task<IPermission?> GetPermission(string name) => await _permissionRepository.GetPermissionAsync(name);

    public Task AddPermission(IPermission permission) => _permissionRepository.AddPermissionAsync(permission);

    public Task UpdatePermission(IPermission permission) => _permissionRepository.UpdatePermissionAsync(permission);

    public async Task RemovePermission(string name)
    {
        var permission = await GetPermission(name) as DbPermission;

        if (permission == null)
        {
            throw new InvalidOperationException($"Permission with name '{name}' does not exist.");
        }

        await _permissionRepository.RemovePermissionAsync(permission);
    }

    public Task RemovePermission(IPermission permission) => RemovePermission(permission.Name);

    public Task AddGroup(IGroup group) => _permissionRepository.AddGroupAsync(group);

    public async Task RemoveGroup(int id)
    {
        var group = await GetGroup(id) as DbGroup;

        if (group == null)
        {
            throw new InvalidOperationException($"Group with id {id} does not exist.");
        }

        await _permissionRepository.RemoveGroupAsync(group);
    }

    public Task RemoveGroup(IGroup group) => RemoveGroup(group.Id);

    public Task UpdateGroup(IGroup group) => _permissionRepository.UpdateGroupAsync(group);

    public async Task<IGroup?> GetGroup(int id) => await _permissionRepository.GetGroup(id);

    public async Task AddPlayerToGroup(IPlayer player, IGroup group) =>
        await _permissionRepository.AddPlayerToGroupAsync(player.Id, group.Id);

    public async Task RemovePlayerFromGroup(IPlayer player, IGroup group) =>
        await _permissionRepository.RemovePlayerFromGroupAsync(player.Id);

    public async Task AddPermissionToGroup(IGroup group, IPermission permission) =>
        await _permissionRepository.AddPermissionToGroupAsync(group.Id, permission.Id);

    public async Task RemovePermissionFromGroup(IGroup group, IPermission permission) =>
        await _permissionRepository.RemovePermissionFromGroupAsync(group.Id, permission.Id);

    public async Task ClearGroupPermissions(IGroup group) =>
        await _permissionRepository.ClearGroupPermissionsAsync(group.Id);
}
