using System.Data.Common;
using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using RepoDb;

namespace EvoSC.Common.Database.Repository.Permissions;

public class PermissionRepository : EvoScDbRepository<DbPermission>, IPermissionRepository
{
    public PermissionRepository(IDbConnectionFactory connectionFactory) : base(
        connectionFactory)
    {
    }

    public async Task<IEnumerable<IGroup>> GetGroupsAsync(long playerId)
    {
        var groupsSql = """ 
            SELECT gs.* FROM ""Groups"" gs
            INNER JOIN ""UserGroups"" ug ON ug.GroupID=gs.Id
            WHERE ug.UserId=@UserId
            """;
        var groupsValues = new { UserId = playerId };
        return await Database.ExecuteQueryAsync<DbGroup>(groupsSql, groupsValues);
    }

    public async Task AddPermissionAsync(IPermission permission) => await Database.InsertAsync(new DbPermission(permission));

    public async Task UpdatePermissionAsync(IPermission permission) =>
        await Database.UpdateAsync(new DbPermission(permission));

    public async Task<IPermission?> GetPermissionAsync(string name)
    {
        var result = await Database.QueryAsync<DbPermission>(e => e.Name == name);

        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<IPermission>> GetPlayerPermissionsAsync(long playerId)
    {
        var sql = """
            SELECT prms.Name FROM Players ps
            INNER JOIN ""UserGroups"" ug ON ug.UserId=ps.Id
            INNER JOIN ""GroupPermissions"" gp ON gp.GroupId=ug.GroupID
            INNER JOIN ""Permissions"" prms ON prms.Id=gp.PermissionId
            WHERE ps.Id=@UserId
            """;
        var values = new { UserId = playerId };
        return await Database.ExecuteQueryAsync<DbPermission>(sql, values);
    }

    public async Task RemovePermissionAsync(IPermission permission)
    {
        var queryDeleteGroupPermissions = "DELETE FROM GroupPermissions WHERE PermissionId=@PermissionId";
        var valuesDeleteGroupPermissions = new { PermissionId = permission.Id };
        await Database.QueryAsync(queryDeleteGroupPermissions, valuesDeleteGroupPermissions);
        await Database.DeleteAsync(permission);
    }

    public async Task AddGroupAsync(IGroup group) => await Database.InsertAsync(new DbGroup(group));

    public async Task UpdateGroupAsync(IGroup group) => await Database.UpdateAsync(new DbGroup(group));

    public async Task RemoveGroupAsync(IGroup group)
    {
        // group-permission relation
        var queryDeleteGroupPermissions = "DELETE FROM GroupPermissions WHERE GroupId=@GroupId";
        var valuesDeleteGroupPermissions = new { GroupId = group.Id };
        await Database.ExecuteQueryAsync(queryDeleteGroupPermissions, valuesDeleteGroupPermissions);

        // user-group relation
        var queryDeleteUserGroups = "DELETE FROM UserGroups WHERE GroupId=@GroupId";
        var valuesDeleteUserGroups = new { GroupId = group.Id };
        await Database.ExecuteQueryAsync(queryDeleteUserGroups, valuesDeleteUserGroups);

        // group info itself
        await Database.DeleteAsync(group);
    }

    public async Task<IGroup?> GetGroup(int id)
    {
        /*var query = """
        SELECT Groups.*, Permissions.* FROM Groups
        LEFT JOIN GroupPermissions ON GroupPermissions.GroupId=Groups.Id
        LEFT JOIN Permissions ON Permissions.Id=GroupPermissions.PermissionId
        WHERE Groups.Id=@GroupId
        """;
        var values = new {GroupId = id};
        var groups = await Database.QueryAsync<DbGroup, DbPermission, DbGroup>(query, (group, permission) =>
        {
            if (permission != null)
            {
                group.Permissions.Add(permission);
            }
            
            return group;
        }, values);
        
        var result = groups.GroupBy(g => g.Id).Select(g =>
        {
            var grouped = g.First();
            grouped.Permissions = g
                .Where(p => p.Permissions != null && p.Permissions.Count > 0)
                .Select(p => p.Permissions.Single())
                .ToList();
            
            return grouped;
        });
        
        return result.FirstOrDefault();*/
        return null;
    }

    public async Task AddPlayerToGroupAsync(long playerId, int groupId) =>
        await Database.InsertAsync(new DbUserGroup { GroupId = groupId, UserId = playerId });

    public async Task RemovePlayerFromGroupAsync(long playerId)
    {
        var sql = "DELETE FROM UserGroups WHERE UserId=@UserId";
        var values = new { UserId = playerId };
        await Database.ExecuteQueryAsync(sql, values);
    }

    public async Task AddPermissionToGroupAsync(int groupId, int permissionId)
    {
        var sql = "INSERT INTO GroupPermissions(PermissionId, GroupId) VALUES(@PermissionId, @GroupId)";
        var values = new { PermissionId = permissionId, GroupId = groupId };
        await Database.ExecuteQueryAsync(sql, values);
    }

    public Task RemovePermissionFromGroupAsync(int groupId, int permissionId)
    {
        var sql = "DELETE FROM GroupPermissions WHERE PermissionId=@PermissionId AND GroupId=@GroupId";
        var values = new { PermissionId = permissionId, GroupId = groupId };
        return Database.ExecuteQueryAsync(sql, values);
    }

    public Task ClearGroupPermissionsAsync(int groupId)
    {
        var sql = "DELETE FROM GroupPermissions WHERE GroupId=@GroupId";
        var values = new { GroupId = groupId };
        return Database.ExecuteQueryAsync(sql, values);
    }
}
