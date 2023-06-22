using System.Dynamic;
using System.Resources;

namespace EvoSC.Common.Interfaces.Localization;

public abstract class ILocale : DynamicObject
{
    /// <summary>
    /// Get the string of a locale key.
    /// </summary>
    /// <param name="name">Name of the locale</param>
    /// <param name="args">Any formatting arguments to pass to string.Format</param>
    public abstract string this[string name, params object[] args] { get; }
    
    /// <summary>
    /// Use the player's selected language when returning locale strings.
    /// </summary>
    public abstract ILocale PlayerLanguage { get; }
    
    /// <summary>
    /// Get the resource set for the current resource.
    /// </summary>
    /// <returns></returns>
    public abstract ResourceSet? GetResourceSet();
    
    /// <summary>
    /// Translate a string pattern containing locale names.
    /// </summary>
    /// <param name="pattern">The string to translate. Any locale name in the format
    /// [LocaleName] will be replaced.</param>
    /// <param name="args">Any formatting arguments to pass to string.Format</param>
    /// <returns></returns>
    public abstract string Translate(string pattern, params object[] args);
}
