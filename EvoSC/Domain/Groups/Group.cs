using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvoSC.Domain.Groups;

[Table("Groups")]
public class Group
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Prefix { get; set; }

    public string Color { get; set; }

    public IEnumerable<Permission> Permissions { get; set; }
}
