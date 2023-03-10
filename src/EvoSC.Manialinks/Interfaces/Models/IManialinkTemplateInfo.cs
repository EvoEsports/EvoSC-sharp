using System.Reflection;

namespace EvoSC.Manialinks.Interfaces.Models;

public interface IManialinkTemplateInfo
{
    public IEnumerable<Assembly> Assemblies { get; }
    public string Name { get; }
    public string Content { get; }
}
