namespace EvoSC.Modules.Official.HelpModule.Models;

public class HelpCommandInfo
{
    public required string Usage { get; init; }
    public required string Description { get; init; }
    public string? Permission { get; init; }
}
