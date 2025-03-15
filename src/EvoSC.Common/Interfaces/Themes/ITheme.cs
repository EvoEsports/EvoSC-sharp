namespace EvoSC.Common.Interfaces.Themes;

public interface ITheme
{
    /// <summary>
    /// Configures the theme.
    /// </summary>
    /// <param name="theme">Current effective theme options.</param>
    /// <returns></returns>
    public Task ConfigureAsync(dynamic theme);

    /// <summary>
    /// Configure dynamic theme options such that rely on certain conditions
    /// and may use it's own default options for those tasks.
    /// </summary>
    /// <param name="theme">Current effective theme options.</param>
    /// <returns></returns>
    public Task ConfigureDynamicAsync(dynamic theme);
    
    /// <summary>
    /// Options set by this theme.
    /// </summary>
    public Dictionary<string, object> ThemeOptions { get; }
    
    /// <summary>
    /// Component replacements defined by this theme.
    /// </summary>
    public Dictionary<string, string> ComponentReplacements { get; }
}
    
