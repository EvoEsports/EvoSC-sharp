
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Permissions;

[Table("UserGroups")]
public class DbUserGroup
{
    [Column]
    public long UserId { get; set; }
    
    [Column]
    public int GroupId { get; set; }
    
    [Column]
    public bool Display { get; set; }
}
