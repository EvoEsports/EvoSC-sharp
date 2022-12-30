using System.Data.Common;
using Dapper;
using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Interfaces.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Database.Repository.Permissions;

public class PermissionRepository
{
    private readonly ILogger<PermissionRepository> _logger;
    private readonly DbConnection _db;

    public PermissionRepository(ILogger<PermissionRepository> logger, DbConnection db)
    {
        _logger = logger;
        _db = db;
    }
    
    public async Task<IEnumerable<DbPermission>?> GetPermissions(long playerId)
    {
        var sql = """
            SELECT prms.Name FROM Players ps
            INNER JOIN UserGroups ug ON ug.UserId=ps.Id
            INNER JOIN GroupPermissions gp ON gp.GroupId=ug.GroupID
            INNER JOIN Permissions prms ON prms.Id=gp.PermissionId
            WHERE ps.Id=@UserId
            """;
        var values = new {UserId = playerId};
        return await _db.QueryAsync<DbPermission>(sql, values);
    }
}
