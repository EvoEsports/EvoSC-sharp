using EvoSC.Common.Interfaces.Models;
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Permissions;

[Table("Groups")]
public class DbGroup : IGroup
{
    [PrimaryKey, Identity]
    public int Id { get; set; }
    
    [Column]
    public string Title { get; set; }
    
    [Column]
    public string Description { get; set; }
    
    [Column]
    public string? Icon { get; set; }
    
    [Column]
    public string? Color { get; set; }
    
    [Column]
    public bool Unrestricted { get; set; }
    
    [Association(ThisKey = nameof(DbGroupPermission.GroupId), OtherKey = nameof(DbGroupPermission.PermissionId))]
    public List<IPermission> Permissions { get; set; }

    public DbGroup()
    {
        Permissions = new List<IPermission>();
    }

    public DbGroup(IGroup group)
    {
        Id = group.Id;
        Title = group.Title;
        Description = group.Description;
        Icon = group.Icon;
        Color = group.Color;
        Unrestricted = group.Unrestricted;
        Permissions = group.Permissions;
    }
}
