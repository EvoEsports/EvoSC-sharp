using RepoDb.Attributes;

namespace EvoSC.Common.Database.Models.Permissions;

[Map("UserGroups")]
public class DbUserGroup
{
    public long UserId { get; set; }
    public int GroupId { get; set; }
    public bool Display { get; set; }
}
