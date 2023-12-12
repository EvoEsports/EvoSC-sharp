using EvoSC.Common.Themes;

namespace EvoSC.Common.Interfaces.Themes;

public interface IThemeManager
{
    /// <summary>
    /// All available themes.
    /// </summary>
    public IEnumerable<IThemeInfo> AvailableThemes { get; }
    
    /// <summary>
    /// Options for the current theme.
    /// </summary>
    public dynamic Theme { get; }
    
    /// <summary>
    /// All available component replacements.
    /// </summary>
    public Dictionary<string, string> ComponentReplacements { get; }
    
    /// <summary>
    /// Add a new theme.
    /// </summary>
    /// <param name="themeType">Class type of the theme.</param>
    /// <param name="moduleId">ID of the module providing the theme.</param>
    /// <returns></returns>
    public Task AddThemeAsync(Type themeType, Guid moduleId);
    
    /// <summary>
    /// Add a new theme.
    /// </summary>
    /// <param name="themeType">Class type of the theme.</param>
    /// <returns></returns>
    public Task AddThemeAsync(Type themeType);

    /// <summary>
    /// Add a new theme.
    /// </summary>
    /// <typeparam name="TTheme">Type of the theme class.</typeparam>
    /// <returns></returns>
    public Task AddThemeAsync<TTheme>() where TTheme : Theme<TTheme>
        => AddThemeAsync(typeof(TTheme));

    /// <summary>
    /// Add a new theme.
    /// </summary>
    /// <param name="moduleId">Id of the module providing the theme.</param>
    /// <typeparam name="TTheme">Type of the theme class.</typeparam>
    /// <returns></returns>
    public Task AddThemeAsync<TTheme>(Guid moduleId) where TTheme : Theme<TTheme>
        => AddThemeAsync(typeof(TTheme), moduleId);
    
    /// <summary>
    /// Remove a theme.
    /// </summary>
    /// <param name="name">Name of the theme to remove.</param>
    /// <returns></returns>
    public Task RemoveThemeAsync(string name);
    
    /// <summary>
    /// Remove all themes from a module.
    /// </summary>
    /// <param name="moduleId">ID of the module to remove themes from.</param>
    /// <returns></returns>
    public Task RemoveThemesForModuleAsync(Guid moduleId);
    
    /// <summary>
    /// Activate a theme. This will replace existing themes which this theme will
    /// potentially override.
    /// </summary>
    /// <param name="name">Name of the theme to activate.</param>
    /// <returns></returns>
    public Task<ITheme> ActivateThemeAsync(string name);
    
    /// <summary>
    /// Invalidate the theme options cache and renew option values.
    /// </summary>
    public void InvalidateCache();
}
