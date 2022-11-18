using Dapper.Contrib.Extensions;

namespace EvoSC.Common.Database.Models;

[Table("UserGroups")]
public class DbUserGroup
{
    public long UserId { get; set; }
    public int GroupId { get; set; }
    public bool Display { get; set; }
}
