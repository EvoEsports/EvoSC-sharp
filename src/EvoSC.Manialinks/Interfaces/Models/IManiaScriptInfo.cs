namespace EvoSC.Manialinks.Interfaces.Models;

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
