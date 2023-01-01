using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Interfaces.Repository;

public interface IPermissionRepository
{
    public Task AddPermissionAsync(IPermission permission);

    public Task UpdatePermissionAsync(IPermission permission);

    public Task<IPermission?> GetPermissionAsync(string name);
    
    public Task<IEnumerable<IPermission>> GetPlayerPermissionsAsync(long playerId);

    public Task<IEnumerable<IGroup>> GetGroupsAsync(long playerId);

    public Task RemovePermissionAsync(IPermission permission);
}
