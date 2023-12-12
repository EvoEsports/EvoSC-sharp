namespace EvoSC.Common.Themes.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ThemeAttribute : Attribute
{
    /// <summary>
    /// Unique name of the theme.
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// Short summary describing the theme.
    /// </summary>
    public required string Description { get; init; }
    
    /// <summary>
    /// The class of the theme which this theme will override.
    /// </summary>
    public Type? OverrideTheme { get; init; }
}
