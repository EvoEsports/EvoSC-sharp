using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Interfaces.Database.Repository;

public interface IPermissionRepository
{
    public Task AddPermissionAsync(IPermission permission);

    public Task UpdatePermissionAsync(IPermission permission);

    public Task<IPermission?> GetPermissionAsync(string name);
    
    public Task<IEnumerable<IPermission>> GetPlayerPermissionsAsync(long playerId);
    
    public Task RemovePermissionAsync(IPermission permission);

    public Task<IEnumerable<IGroup>> GetGroupsAsync(long playerId);

    public Task AddGroupAsync(IGroup group);

    public Task UpdateGroupAsync(IGroup group);

    public Task RemoveGroupAsync(IGroup group);

    public Task<IGroup?> GetGroupAsync(int id);

    public Task AddPlayerToGroupAsync(long playerId, int groupId);

    public Task RemovePlayerFromGroupAsync(long playerId, int groupId);

    public Task AddPermissionToGroupAsync(int groupId, int permissionId);

    public Task RemovePermissionFromGroupAsync(int groupId, int permissionId);

    public Task ClearGroupPermissionsAsync(int groupId);
}
