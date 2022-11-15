using EvoSC.Common.Database.Models;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Interfaces.Services;

public interface IPermissionManager
{
    public Task<bool> HasPermissionAsync(IPlayer player, string permission);
    public Task<IPermission?> GetPermission(string name);
    public Task AddPermission(IPermission permission);
    public Task UpdatePermission(IPermission permission);
    public Task RemovePermission(string name);
    public Task RemovePermission(IPermission permission);
    public Task AddGroup(IGroup group);
    public Task RemoveGroup(int id);
    public Task UpdateGroup(IGroup group);
    public Task<IGroup?> GetGroup(int id);
    public Task AssignGroup(IPlayer player, IGroup group);
    public Task AddPermissionToGroup(IPermission permission, IGroup group);
    public Task RemovePermissionFromGroup(IPermission permission, IGroup group);
    public Task ClearGroupPermissions(IGroup group);
}
