namespace EvoSC.Manialinks.Interfaces.Models;

/// <summary>
/// Holds information about a ManiaScript content object.
/// </summary>
public interface IManiaScriptInfo
{
    /// <summary>
    /// The full qualified name of this ManiaScript.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// The content code of this ManiaScript.
    /// </summary>
    public string Content { get; }
}
