namespace EvoSC.Modules.Official.ShowCommandsModule.Models;

public class CommandInfo
{
    public required string Usage { get; init; }
    public required string Description { get; init; }
    public string? Permission { get; init; }
}
