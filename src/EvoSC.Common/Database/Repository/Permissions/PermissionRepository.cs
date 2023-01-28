using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using LinqToDB;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Database.Repository.Permissions;

public class PermissionRepository : DbRepository, IPermissionRepository
{
    private readonly ILogger<PermissionRepository> _logger;
    
    public PermissionRepository(IDbConnectionFactory dbConnFactory, ILogger<PermissionRepository> logger) : base(dbConnFactory)
    {
        _logger = logger;
    }

    public async Task<IPermission> AddPermissionAsync(IPermission permission)
    {
        var id = await Database.InsertWithIdentityAsync(new DbPermission(permission));
        var dbPerm = new DbPermission(permission) {Id = Convert.ToInt32(id)};
        return dbPerm;
    }

    public Task UpdatePermissionAsync(IPermission permission) => Database.UpdateAsync(new DbPermission(permission));

    public async Task<IPermission?> GetPermissionAsync(string name) => await Table<DbPermission>()
        .SingleOrDefaultAsync(p => p.Name == name);

    public async Task<IEnumerable<IPermission>> GetPlayerPermissionsAsync(long playerId) => await
        (
            from p in Table<DbPlayer>()
            join ug in Table<DbUserGroup>() on p.Id equals ug.UserId
            join gp in Table<DbGroupPermission>() on ug.GroupId equals gp.GroupId
            join ps in Table<DbPermission>() on gp.PermissionId equals ps.Id
            where p.Id == playerId
            select ps
        )
        .ToArrayAsync();

    public Task RemovePermissionAsync(IPermission permission) => Database.DeleteAsync(new DbPermission(permission));

    public async Task<IEnumerable<IGroup>> GetGroupsAsync(long playerId) => await
        (
            from g in Table<DbGroup>()
            join ug in Table<DbUserGroup>() on g.Id equals ug.GroupId
            where ug.UserId == playerId
            select g
        )
        .ToArrayAsync();

    public async Task<IGroup> AddGroupAsync(IGroup group)
    {
        var id = await Database.InsertWithIdentityAsync(new DbGroup(group));
        var dbGroup = new DbGroup(group) {Id = Convert.ToInt32(id)};

        return dbGroup;
    }

    public Task UpdateGroupAsync(IGroup group) => Database.UpdateAsync(new DbGroup(group));

    public async Task RemoveGroupAsync(IGroup group)
    {
        await using var transaction = await Database.BeginTransactionAsync();
        try
        {
            await Table<DbGroupPermission>().DeleteAsync(t => t.GroupId == group.Id);
            await Table<DbUserGroup>().DeleteAsync(t => t.GroupId == group.Id);
            await Table<DbGroupPermission>().DeleteAsync(t => t.GroupId == group.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove group");
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IGroup?> GetGroupAsync(int id) => await Table<DbGroup>().SingleOrDefaultAsync(t => t.Id == id);

    public Task AddPlayerToGroupAsync(long playerId, int groupId) => Database.InsertAsync(new DbUserGroup
    {
        UserId = playerId, GroupId = groupId, Display = false
    });

    public Task RemovePlayerFromGroupAsync(long playerId, int groupId) => Table<DbUserGroup>()
        .DeleteAsync(t => t.UserId == playerId && t.GroupId == groupId);

    public Task AddPermissionToGroupAsync(int groupId, int permissionId) => Database.InsertAsync(new DbGroupPermission
    {
        GroupId = groupId, PermissionId = permissionId
    });

    public Task RemovePermissionFromGroupAsync(int groupId, int permissionId) => Table<DbGroupPermission>()
        .DeleteAsync(t => t.GroupId == groupId && t.PermissionId == permissionId);

    public Task ClearGroupPermissionsAsync(int groupId) => Table<DbGroupPermission>()
        .DeleteAsync(t => t.GroupId == groupId);
}
