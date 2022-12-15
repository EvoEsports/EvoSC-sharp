using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Models;

public class ModuleDependency : IModuleDependency
{
    public required string Name { get; init; }
    public required Version Version { get; init; }
}
