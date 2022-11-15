using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Database.Models;
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

    public Task AddPermission(IPermission permission) => _db.InsertAsync(permission);

    public Task UpdatePermission(IPermission permission)
    {
        var query = "UPDATE `Permissions` SET `Name`=@Name, `Description`=@Description";
        var values = new {permission.Name, permission.Description};

        return _db.QueryAsync(query, values);
    }

    public async Task RemovePermission(string name)
    {
        var permission = await GetPermission(name) as DbPermission;

        if (permission == null)
        {
            throw new InvalidOperationException($"Permission with name {name} does not exist.");
        }

        var queryDeleteGroupPermissions = "DELETE FROM `GroupPermissions` WHERE PermissionId=@PermissionId";
        var valuesDeleteGroupPermissions = new {PermissionId = permission.Id};
        await _db.QueryAsync(queryDeleteGroupPermissions, valuesDeleteGroupPermissions);
        await _db.DeleteAsync(permission);
    }

    public Task RemovePermission(IPermission permission) => RemovePermission(permission.Name);

    public async Task AddGroup(IGroup group)
    {
        await _db.InsertAsync(group);
    }

    public async Task RemoveGroup(int id)
    {
        var group = await GetGroup(id) as DbGroup;

        if (group == null)
        {
            throw new InvalidOperationException($"Group with id {id} does not exist.");
        }
        
        var queryDeleteGroupPermissions = "DELETE FROM `GroupPermissions` WHERE `GroupId`=@GroupId";
        var valuesDeleteGroupPermissions = new {GroupId = group.Id};
        await _db.QueryAsync(queryDeleteGroupPermissions, valuesDeleteGroupPermissions);
        await _db.DeleteAsync(group);
    }

    public Task UpdateGroup(IGroup group)
    {
        var query = """
        UPDATE `Permissions` SET 
        `Title`=@Name, 
        `Description`=@Description
        `Icon`=@Icon
        `Color`=@Color
        `Unrestricted`=@Unrestricted 
        """;
        
        var values = new
        {
            group.Title,
            group.Description,
            group.Icon,
            group.Color,
            group.Unrestricted
        };

        return _db.QueryAsync(query, values);
    }

    public async Task<IGroup?> GetGroup(int id)
    {
        var query = """
        SELECT `Groups`.*, `Permissions`.* FROM `Groups`
        INNER JOIN `GroupPermissions` ON `GroupPermissions`.`GroupId`=`Groups`.`Id`
        INNER JOIN `Permissions` ON `Permissions`.`Id`=`GroupPermissions`.`PermissionId`
        WHERE `Groups`.`Id`=@GroupId
        """;
        var values = new {GroupId = id};
        var groups = await _db.QueryAsync<DbGroup, DbPermission, DbGroup>(query, (group, permission) =>
        {
            group.Permissions.Add(permission);
            return group;
        }, values);

        var result = groups.GroupBy(g => g.Id).Select(g =>
        {
            var grouped = g.First();
            grouped.Permissions = g.Select(p => p.Permissions.Single()).ToList();
            return grouped;
        });
        
        return result.FirstOrDefault();
    }

    public async Task AssignGroup(IPlayer player, IGroup group)
    {
        var queryPlayer = "SELECT * FROM `Players` WHERE `AccountId`=@AccountId";
        var valuesPlayer = new {player.AccountId};
        var dbPlayer = (await _db.QueryAsync<DbPlayer>(queryPlayer, valuesPlayer)).FirstOrDefault();

        if (dbPlayer == null)
        {
            throw new InvalidOperationException($"Failed to find player with account id: {player.AccountId}");
        }

        await _db.InsertAsync(new DbUserGroup {GroupId = group.Id, UserId = dbPlayer.Id});
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
