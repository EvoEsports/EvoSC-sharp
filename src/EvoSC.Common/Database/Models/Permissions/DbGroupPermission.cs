
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Permissions;

[Table(TableName)]
public class DbGroupPermission
{
    public const string TableName = "GroupPermissions";
    
    [Column]
    public int GroupId { get; set; }
    
    [Column]
    public int PermissionId { get; set; }
}
