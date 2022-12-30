using Dapper.Contrib.Extensions;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Database.Models;

[Table("\"Groups\"")]
public class DbGroup : IGroup
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public bool Unrestricted { get; set; }
    
    [Computed]
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
