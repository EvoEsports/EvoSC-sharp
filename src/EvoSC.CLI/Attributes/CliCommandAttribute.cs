namespace EvoSC.CLI.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CliCommandAttribute : Attribute
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}
