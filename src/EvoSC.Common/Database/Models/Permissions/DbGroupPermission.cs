
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Permissions;

[Table("GroupPermissions")]
public class DbGroupPermission
{
    [Column]
    public int GroupId { get; set; }
    
    [Column]
    public int PermissionId { get; set; }
}
