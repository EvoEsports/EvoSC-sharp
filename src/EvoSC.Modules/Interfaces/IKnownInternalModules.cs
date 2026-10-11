namespace EvoSC.Modules.Interfaces;

/// <summary>
/// The ids of the modules the application was built to recognize as internal. Registered once by
/// the host from a list fixed in code, not from configuration: an admin editing a config file must
/// not be able to grant a module the protection from unload/reload that internal status carries.
/// </summary>
public interface IKnownInternalModules
{
    /// <summary>
    /// Whether <paramref name="moduleId"/> is one of the ids the application knows about.
    /// </summary>
    public bool Contains(string moduleId);
}
