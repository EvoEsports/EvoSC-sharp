namespace EvoSC.Common.Interfaces.Themes;

public interface IThemeInfo
{
    /// <summary>
    /// The class type of the theme.
    /// </summary>
    public Type ThemeType { get; }
    
    /// <summary>
    /// Unique name of the theme.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Short summary describing the theme.
    /// </summary>
    public string Description { get; }
    
    /// <summary>
    /// Class of the theme which this theme overrides.
    /// </summary>
    public Type? OverrideTheme { get; }
    
    /// <summary>
    /// ID of the module this theme is part of.
    /// </summary>
    public Guid ModuleId { get; }
    
    /// <summary>
    /// The effective theme class that is used.
    /// </summary>
    public Type EffectiveThemeType { get; }
}
