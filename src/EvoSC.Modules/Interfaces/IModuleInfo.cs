namespace EvoSC.Modules.Interfaces;

public interface IModuleInfo
{
    /// <summary>
    /// The unique identifier name for the module.
    /// 
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// The title of the module.
    /// </summary>
    public string Title { get; }
    
    /// <summary>
    /// Short description of the module.
    /// </summary>
    public string Summary { get; }
    
    /// <summary>
    /// The module's current version.
    /// </summary>
    public Version Version { get; }
    
    /// <summary>
    /// The author of the module.
    /// </summary>
    public string Author { get; }
    
    /// <summary>
    /// Other modules that this module depends on.
    /// </summary>
    public IEnumerable<IModuleDependency> Dependencies { get; }
    
    /// <summary>
    /// Whether this module is internal or not.
    /// </summary>
    public bool IsInternal { get; }
}
