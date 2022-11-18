using Dapper.Contrib.Extensions;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Database.Models;

[Table("Permissions")]
public class DbPermission : IPermission
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public DbPermission(){}

    public DbPermission(IPermission permission)
    {
        Id = permission.Id;
        Name = permission.Name;
        Description = permission.Description;
    }
}
