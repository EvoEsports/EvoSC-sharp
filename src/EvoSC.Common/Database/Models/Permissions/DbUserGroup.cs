
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Permissions;

[Table(TableName)]
public class DbUserGroup
{
    public const string TableName = "UserGroups";
    
    [Column]
    public long UserId { get; set; }
    
    [Column]
    public int GroupId { get; set; }
    
    [Column]
    public bool Display { get; set; }
}
