namespace EvoSC.Common.Themes.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ThemeAttribute : Attribute
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public Type? OverrideTheme { get; init; }
}
