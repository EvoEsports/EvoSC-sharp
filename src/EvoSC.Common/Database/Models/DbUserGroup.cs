using System.ComponentModel.DataAnnotations.Schema;

namespace EvoSC.Common.Database.Models;

[Dapper.Contrib.Extensions.Table("\"UserGroups\"")]
public class DbUserGroup
{
    [Column("\"UserId\"")]
    public long UserId { get; set; }
    [Column("\"GroupId\"")]
    public int GroupId { get; set; }
    [Column("\"Display\"")]
    public bool Display { get; set; }
}
