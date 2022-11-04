namespace EvoSC.Commands.Interfaces;

public interface IChatCommand
{
    public string Name { get; }
    public string Description { get; }
    public string? Permission { get; }
    public IEnumerable<string> Aliases { get; }
}
