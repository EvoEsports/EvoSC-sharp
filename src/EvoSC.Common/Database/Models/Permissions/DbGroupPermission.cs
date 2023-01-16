using System.ComponentModel.DataAnnotations.Schema;

namespace EvoSC.Common.Database.Models.Permissions;

[Table("GroupPermissions")]
public class DbGroupPermission
{
    public int GroupId { get; set; }
    public int PermissionId { get; set; }
}
