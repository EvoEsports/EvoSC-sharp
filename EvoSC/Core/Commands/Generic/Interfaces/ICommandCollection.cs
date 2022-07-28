using System.Collections.Generic;

namespace EvoSC.Core.Commands.Generic.Interfaces;

public interface ICommandCollection<TCmd>
{
    public IReadOnlyList<TCmd> Commands { get; }

    public void AddCommand(TCmd cmd);
    public bool RemoveCommand(string name, string? group=null);
    public bool RemoveGroup(string group);
    public TCmd GetCommand(string name, string? group=null);
    public bool CommandExists(string name, string? group=null);
}
