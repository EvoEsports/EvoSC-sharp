namespace EvoSC.CLI.Attributes;

/// <summary>
/// Define a class as a CLI command handler.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CliCommandAttribute : Attribute
{
    /// <summary>
    /// The name of the CLI command. Must alphanumeric.
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// A short summary describing what this command does.
    /// </summary>
    public required string Description { get; init; }
}
