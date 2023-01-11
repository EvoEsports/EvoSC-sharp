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

namespace EvoSC.Common.Database.Repository.Permissions;

public class PermissionRepository : EvoScDbRepository<DbPermission>, IPermissionRepository
{
    public PermissionRepository(IDbConnectionFactory connectionFactory) : base(
        connectionFactory)
    {
    }

    public async Task<IEnumerable<IGroup>> GetGroupsAsync(long playerId)
    {
        var query = NewQuery()
            .Select()
            .WriteText("gs.*")
            .From()
            .TableNameFrom<DbGroup>(DatabaseSetting).WriteText("gs")
            .InnerJoin()
            .TableNameFrom<DbUserGroup>(DatabaseSetting).WriteText("ug")
            .On()
            .WriteText($"ug.{Quote("GroupID")}")
            .WriteText("=")
            .WriteText($"gs.{Quote("Id")}")
            .Where()
            .WriteText($"ug.{Quote("UserId")}")
            .WriteText("=")
            .WriteText("@UserId");

        return await Database.ExecuteQueryAsync<DbGroup>(query.ToString(), new {UserId = playerId});
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
        var query = NewQuery()
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
            .WriteText($"ps.{Quote("Id")}").WriteText("=").WriteText("@UserId");

        return await Database.ExecuteQueryAsync<DbPermission>(query.ToString(), new {UserId = playerId});
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
        var query = NewQuery()
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
            .End();

        var extractor = await Database.ExecuteQueryMultipleAsync(query.ToString(), new {Id = id, GroupId = id});
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
