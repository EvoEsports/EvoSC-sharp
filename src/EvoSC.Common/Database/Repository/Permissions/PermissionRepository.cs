using System.Data.Common;
using EvoSC.Common.Database.Extensions;
using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using RepoDb;
using RepoDb.Extensions;
using SqlKata;

namespace EvoSC.Common.Database.Repository.Permissions;

public class PermissionRepository : EvoScDbRepository<DbPermission>, IPermissionRepository
{
    public PermissionRepository(IDbConnectionFactory connectionFactory) : base(
        connectionFactory)
    {
    }

    public async Task<IEnumerable<IGroup>> GetGroupsAsync(long playerId)
    {
        var (sql, values) = Query("Groups")
            .Join("UserGroup", "UserGroups.GroupID", "Groups.Id")
            .Where("UserGroup.UserId", playerId)
            .Compile();
        
        return await Database.ExecuteQueryAsync<DbGroup>(sql, values);
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
        /* var query = NewQuery()
            .Select()
            .WriteText($"prms.*")
            .From()
            .TableNameFrom<DbPlayer>(DatabaseSetting).WriteText("ps")

            .InnerJoin()
            .TableNameFrom<DbUserGroup>(DatabaseSetting).WriteText("ug")
            .On().WriteText($"ug.{Quote("UserId")}").WriteText("=").WriteText($"ps.{Quote("Id")}")

            .InnerJoin()
            .TableNameFrom<DbGroupPermission>(DatabaseSetting).WriteText("gp")
            .On().WriteText($"gp.{Quote("GroupId")}").WriteText("=").WriteText($"ug.{Quote("GroupId")}")

            .InnerJoin()
            .TableNameFrom<DbPermission>(DatabaseSetting).WriteText("prms")
            .On().WriteText($"prms.{Quote("Id")}").WriteText("=").WriteText($"gp.{Quote("PermissionId")}")
            
            .Where()
            .WriteText($"ps.{Quote("Id")}").WriteText("=").WriteText("@UserId"); */

        var (sql, values) = Query()
            .Select("Permissions.*")
            .From("Players")
            .Join("UserGroups", "UserGroups.UserId", "Players.Id")
            .Join("GroupPermissions", "GroupPermissions.GroupId", "UserGroups.GroupId")
            .Join("Permissions", "Permissions.Id", "GroupPermissions.PermissionId")
            .Where("Players.Id", playerId)
            .Compile();

        return await Database.ExecuteQueryAsync<DbPermission>(sql, values);
    }

    public async Task RemovePermissionAsync(IPermission permission)
    {
        await Database.DeleteAsync<DbGroupPermission>(gp => gp.PermissionId == permission.Id);
        await Database.DeleteAsync<DbPermission>(p => p.Id == permission.Id);
    }

    public async Task AddGroupAsync(IGroup group) => await Database.InsertAsync(new DbGroup(group));

    public async Task UpdateGroupAsync(IGroup group) => await Database.UpdateAsync(new DbGroup(group));

    public async Task RemoveGroupAsync(IGroup group)
    {
        await Database.DeleteAsync<DbGroupPermission>(gp => gp.GroupId == group.Id);
        await Database.DeleteAsync<DbUserGroup>(ug => ug.GroupId == group.Id);
        await Database.DeleteAsync<DbGroup>(g => g.Id == group.Id);
    }

    public async Task<IGroup?> GetGroup(int id)
    {
        /* var query = NewQuery()
             // select the group
             .SelectAllFrom<DbGroup>(DatabaseSetting)
             .Where<DbGroup>(g => g.Id == id, DatabaseSetting)
             .End()
             // select all permissions assigned to this group
             .SelectAllFrom<DbPermission>(DatabaseSetting)
             .WhereIn<DbPermission>(p => p.Id, DatabaseSetting)
             .OpenParen()
             .SelectFieldFrom<DbGroupPermission>(gp => gp.GroupId, DatabaseSetting)
             .Where<DbGroupPermission>(gp => gp.GroupId == id, DatabaseSetting)
             .CloseParen()
             .End(); */

        var (sql, values) = MultiQuery()
            .Add(new Query("Groups")
                .Where("Id", id)
            )
            .Add(new Query("Permissions")
                .WhereIn("Id", new Query("GroupPermissions")
                    .Select("PermissionId")
                    .Where("GroupId", id)
                )
            )
            .Compile();
  
         var extractor = await Database.ExecuteQueryMultipleAsync(sql, values);
         var group = extractor.Extract<DbGroup>().FirstOrDefault();
 
         if (group == null)
         {
             return null;
         }
 
         var permissions = extractor.Extract<DbPermission>() as IEnumerable<IPermission>;
         group.Permissions = permissions.AsList();
 
         return group;
    }

    public async Task AddPlayerToGroupAsync(long playerId, int groupId) =>
        await Database.InsertAsync(new DbUserGroup { GroupId = groupId, UserId = playerId });

    public Task RemovePlayerFromGroupAsync(long playerId) =>
        Database.DeleteAsync<DbUserGroup>(ug => ug.UserId == playerId);

    public Task AddPermissionToGroupAsync(int groupId, int permissionId) =>
        Database.InsertAsync(new DbGroupPermission {GroupId = groupId, PermissionId = permissionId});

    public Task RemovePermissionFromGroupAsync(int groupId, int permissionId) =>
        Database.DeleteAsync<DbGroupPermission>(gp => gp.PermissionId == permissionId && gp.GroupId == groupId);

    public Task ClearGroupPermissionsAsync(int groupId) =>
        Database.DeleteAsync<DbGroupPermission>(gp => gp.GroupId == groupId);
}
