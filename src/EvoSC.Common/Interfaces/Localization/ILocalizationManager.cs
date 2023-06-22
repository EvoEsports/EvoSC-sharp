using System.Globalization;
using System.Resources;

namespace EvoSC.Common.Interfaces.Localization;

public interface ILocalizationManager
{
    /// <summary>
    /// The resource manager for the resource of the locales.
    /// </summary>
    public ResourceManager Manager { get; }
    
    /// <summary>
    /// Get the string of a locale key using the provided culture.
    /// </summary>
    /// <param name="culture">The culture/language to use.</param>
    /// <param name="name">Name of the locale.</param>
    /// <param name="args">Arguments passed to string.Format</param>
    /// <returns></returns>
    public string GetString(CultureInfo culture, string name, params object[] args);
}
