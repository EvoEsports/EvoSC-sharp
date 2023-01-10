using System.ComponentModel.DataAnnotations.Schema;
using RepoDb.Attributes;
using RepoDb.Attributes.Parameter;

namespace EvoSC.Common.Database.Models.Permissions;

[Table("GroupPermissions")]
public class DbGroupPermission
{
    public int GroupId { get; set; }
    public int PermissionId { get; set; }
}
