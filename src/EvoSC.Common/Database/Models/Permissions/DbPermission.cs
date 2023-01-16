using System.ComponentModel.DataAnnotations;
using EvoSC.Common.Interfaces.Models;
using RepoDb.Attributes;

namespace EvoSC.Common.Database.Models.Permissions;

[Map("Permissions")]
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
