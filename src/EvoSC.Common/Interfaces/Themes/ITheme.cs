namespace EvoSC.Common.Interfaces.Themes;

public interface ITheme
{
    /// <summary>
    /// Configures the theme.
    /// </summary>
    /// <returns></returns>
    public Task ConfigureAsync();
    
    /// <summary>
    /// Options set by this theme.
    /// </summary>
    public Dictionary<string, object> ThemeOptions { get; }
    
    /// <summary>
    /// Component replacements defined by this theme.
    /// </summary>
    public Dictionary<string, string> ComponentReplacements { get; }
}
    
