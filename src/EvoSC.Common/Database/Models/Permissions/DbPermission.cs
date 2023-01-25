using EvoSC.Common.Interfaces.Models;
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Permissions;

[Table("Permissions")]
public class DbPermission : IPermission
{
    [PrimaryKey, Identity]
    public int Id { get; set; }
    
    [Column]
    public string Name { get; set; }
    
    [Column]
    public string Description { get; set; }
    
    public DbPermission(){}

    public DbPermission(IPermission permission)
    {
        Id = permission.Id;
        Name = permission.Name;
        Description = permission.Description;
    }
}
