using System.Data.Common;
using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using RepoDb;

namespace EvoSC.Common.Database.Repository.Permissions;

public class PermissionRepository : DbRepository<SqlConnection>, IPermissionRepository
{
    private readonly ILogger<PermissionRepository> _logger;
    private readonly DbConnection _db;

    public PermissionRepository(ILogger<PermissionRepository> logger, DbConnection db) : base(db.ConnectionString)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<IEnumerable<IGroup>> GetGroupsAsync(long playerId)
    {
        var groupsSql = """ 
            SELECT gs.* FROM Groups gs
            INNER JOIN UserGroups ug ON ug.GroupID=gs.Id
            WHERE ug.UserId=@UserId
            """;
        var groupsValues = new { UserId = playerId };
        return await _db.QueryAsync<DbGroup>(groupsSql, groupsValues);
    }

    public async Task AddPermissionAsync(IPermission permission) => await _db.InsertAsync(new DbPermission(permission));

    public async Task UpdatePermissionAsync(IPermission permission) =>
        await _db.UpdateAsync(new DbPermission(permission));

    public async Task<IPermission?> GetPermissionAsync(string name)
    {
        var query = "SELECT * FROM Permissions WHERE Name=@Name LIMIT 1";
        var values = new {Name = name};
        var result = await _db.QueryAsync<DbPermission>(query, values);

        return result.FirstOrDefault();
    }
    
    public async Task<IEnumerable<IPermission>> GetPlayerPermissionsAsync(long playerId)
    {
        var sql = """
            SELECT prms.Name FROM Players ps
            INNER JOIN UserGroups ug ON ug.UserId=ps.Id
            INNER JOIN GroupPermissions gp ON gp.GroupId=ug.GroupID
            INNER JOIN Permissions prms ON prms.Id=gp.PermissionId
            WHERE ps.Id=@UserId
            """;
        var values = new { UserId = playerId };
        return await _db.QueryAsync<DbPermission>(sql, values);
    }

    public async Task RemovePermissionAsync(IPermission permission)
    {
        var queryDeleteGroupPermissions = "DELETE FROM GroupPermissions WHERE PermissionId=@PermissionId";
        var valuesDeleteGroupPermissions = new {PermissionId = permission.Id};
        await _db.QueryAsync(queryDeleteGroupPermissions, valuesDeleteGroupPermissions);
        await _db.DeleteAsync("Permissions", permission);
    }
}
