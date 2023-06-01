using System.Reflection;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Models;

public class ManialinkTemplateInfo : IManialinkTemplateInfo
{
    public required IEnumerable<Assembly> Assemblies { get; init; }
    public required string Name { get; init; }
    public required string Content { get; init; }
}
