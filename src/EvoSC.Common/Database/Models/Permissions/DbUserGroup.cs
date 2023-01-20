
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Permissions;

[Table("UserGroups")]
public class DbUserGroup
{
    [PrimaryKey, Identity]
    public long UserId { get; set; }
    
    [Column]
    public int GroupId { get; set; }
    
    [Column]
    public bool Display { get; set; }
}
