using EvoSC.Commands.Interfaces;

namespace EvoSC.Commands;

public class ChatCommand : IChatCommand
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string? Permission { get; init; }
    public IEnumerable<string> Aliases { get; init; }
}
