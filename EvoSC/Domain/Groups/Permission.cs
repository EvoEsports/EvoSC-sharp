using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EvoSC.Domain.Groups;

[Table("Permissions")]
public class Permission
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
    
    public Group Group { get; set; }
}
