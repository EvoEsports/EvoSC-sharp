using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Permissions.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Permissions;

public class PermissionManager : IPermissionManager
{
    private readonly ILogger<PermissionManager> _logger;
    private readonly DbConnection _db;
    
    public PermissionManager(ILogger<PermissionManager> logger, DbConnection db)
    {
        _logger = logger;
        _db = db;
    }
    
    public async Task<bool> HasPermissionAsync(IPlayer player, string permission)
    {
        var sql = """
            SELECT prms.Name FROM `Players` ps
            INNER JOIN `UserGroups` ug ON ug.UserId=ps.Id
            INNER JOIN `GroupPermissions` gp ON gp.GroupId=ug.GroupID
            INNER JOIN `Permissions` prms ON prms.Id=gp.PermissionId
            WHERE ps.Id=@UserId
            """;
        var values = new {UserId = player.Id};
        var result = await _db.QueryAsync(sql, values);

        if (result == null)
        {
            return false;
        }
        
        var permNames = result.Select(p => p.Name);

        foreach (var permName in permNames)
        {
            if (permName.Equals(permission))
            {
                return true;
            }
        }

        var groupsSql = """ 
            SELECT gs.* FROM `Groups` gs
            INNER JOIN `UserGroups` ug ON ug.GroupID=gs.Id
            WHERE ug.UserId=@UserId
            """;
        var groupsValues = new {UserId = player.Id};
        var groups = await _db.QueryAsync<DbGroup>(groupsSql, groupsValues);

        foreach (var group in groups)
        {
            if (group.Unrestricted)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<IPermission?> GetPermission(string name)
    {
        var query = "SELECT * FROM `Permissions` WHERE `Name`=@Name LIMIT 1";
        var values = new {Name = name};
        var result = await _db.QueryAsync<DbPermission>(query, values);

        return result.FirstOrDefault();
    }

    public Task AddPermission(IPermission permission) => _db.InsertAsync(new DbPermission(permission));

    public Task UpdatePermission(IPermission permission) => _db.UpdateAsync(new DbPermission(permission));

    public async Task RemovePermission(string name)
    {
        var permission = await GetPermission(name) as DbPermission;

        if (permission == null)
        {
            throw new InvalidOperationException($"Permission with name '{name}' does not exist.");
        }

        var queryDeleteGroupPermissions = "DELETE FROM `GroupPermissions` WHERE PermissionId=@PermissionId";
        var valuesDeleteGroupPermissions = new {PermissionId = permission.Id};
        await _db.QueryAsync(queryDeleteGroupPermissions, valuesDeleteGroupPermissions);
        await _db.DeleteAsync(permission);
    }

    public Task RemovePermission(IPermission permission) => RemovePermission(permission.Name);

    public async Task AddGroup(IGroup group) => _db.InsertAsync(new DbGroup(group));

    public async Task RemoveGroup(int id)
    {
        var group = await GetGroup(id) as DbGroup;

        if (group == null)
        {
            throw new InvalidOperationException($"Group with id {id} does not exist.");
        }
        
        // group-permission relation
        var queryDeleteGroupPermissions = "DELETE FROM `GroupPermissions` WHERE `GroupId`=@GroupId";
        var valuesDeleteGroupPermissions = new {GroupId = group.Id};
        await _db.QueryAsync(queryDeleteGroupPermissions, valuesDeleteGroupPermissions);

        // user-group relation
        var queryDeleteUserGroups = "DELETE FROM `UserGroups` WHERE `GroupId`=@GroupId";
        var valuesDeleteUserGroups = new {GroupId = group.Id};
        await _db.QueryAsync(queryDeleteUserGroups, valuesDeleteUserGroups);
        
        // group info itself
        await _db.DeleteAsync(group);
    }

    public Task RemoveGroup(IGroup group) => RemoveGroup(group.Id);

    public Task UpdateGroup(IGroup group) => _db.UpdateAsync(new DbGroup(group));

    public async Task<IGroup?> GetGroup(int id)
    {
        var query = """
        SELECT `Groups`.*, `Permissions`.* FROM `Groups`
        LEFT JOIN `GroupPermissions` ON `GroupPermissions`.`GroupId`=`Groups`.`Id`
        LEFT JOIN `Permissions` ON `Permissions`.`Id`=`GroupPermissions`.`PermissionId`
        WHERE `Groups`.`Id`=@GroupId
        """;
        var values = new {GroupId = id};
        var groups = await _db.QueryAsync<DbGroup, DbPermission, DbGroup>(query, (group, permission) =>
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
        
        return result.FirstOrDefault();
    }

    private async Task<DbPlayer?> GetPlayer(string accountId)
    {
        var queryPlayer = "SELECT * FROM `Players` WHERE `AccountId`=@AccountId";
        var valuesPlayer = new {AccountId = accountId};
        var result = await  _db.QueryAsync<DbPlayer>(queryPlayer, valuesPlayer);
        return result.FirstOrDefault();
    }
    
    public async Task AddPlayerToGroup(IPlayer player, IGroup group)
    {
        var dbPlayer = GetPlayer(player.AccountId);

        if (dbPlayer == null)
        {
            throw new InvalidOperationException($"Failed to find player with account id: {player.AccountId}");
        }

        await _db.InsertAsync(new DbUserGroup {GroupId = group.Id, UserId = dbPlayer.Id});
    }

    public async Task RemovePlayerFromGroup(IPlayer player, IGroup group)
    {
        var dbPlayer = GetPlayer(player.AccountId);

        if (dbPlayer == null)
        {
            throw new InvalidOperationException($"Failed to find player with account id: {player.AccountId}");
        }

        var sql = "DELETE FROM `UserGroups` WHERE `UserId`=@UserId";
        var values = new {UserId = dbPlayer.Id};
        await _db.QueryAsync(sql, values);
    }

    public Task AddPermissionToGroup(IGroup group, IPermission permission)
    {
        var sql = "INSERT INTO `GroupPermissions`(`PermissionId`, `GroupId`) VALUES(@PermissionId, @GroupId)";
        var values = new {PermissionId = permission.Id, GroupId = group.Id};
        return _db.QueryAsync(sql, values);
    }

    public Task RemovePermissionFromGroup(IGroup group, IPermission permission)
    {
        var sql = "DELETE FROM `GroupPermissions` WHERE `PermissionId`=@PermissionId AND `GroupId`=@GroupId";
        var values = new {PermissionId = permission.Id, GroupId = group.Id};
        return _db.QueryAsync(sql, values);
    }

    public Task ClearGroupPermissions(IGroup group)
    {
        var sql = "DELETE FROM `GroupPermissions` WHERE `GroupId`=@GroupId";
        var values = new {GroupId = group.Id};
        return _db.QueryAsync(sql, values);
    }
}
