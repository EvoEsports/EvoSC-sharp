using System.Reflection;

namespace EvoSC.Manialinks.Interfaces.Models;

/// <summary>
/// Holds information about a Manialink template.
/// </summary>
public interface IManialinkTemplateInfo
{
    /// <summary>
    /// Assemblies required to render this template.
    /// </summary>
    public IEnumerable<Assembly> Assemblies { get; }
    
    /// <summary>
    /// The fully qualified name of this template.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// The content of this template.
    /// </summary>
    public string Content { get; }
}
