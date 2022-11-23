using EvoSC.Common.Database.Models;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Permissions.Models;

public class Group : IGroup
{
    public int Id { get; init; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public bool Unrestricted { get; set; }
    
    public List<IPermission> Permissions { get; init; }
}
