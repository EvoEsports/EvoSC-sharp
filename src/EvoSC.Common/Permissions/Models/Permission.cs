using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Permissions.Models;

public class Permission : IPermission
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}
