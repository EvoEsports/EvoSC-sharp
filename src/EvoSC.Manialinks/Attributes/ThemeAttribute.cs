namespace EvoSC.Manialinks.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ThemeAttribute : Attribute
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}
