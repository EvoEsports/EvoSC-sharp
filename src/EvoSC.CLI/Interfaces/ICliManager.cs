using System.Reflection;

namespace EvoSC.CLI.Interfaces;

public interface ICliManager
{
    public ICliManager RegisterCommands(Assembly assembly);
    public ICliManager RegisterCommand(Type cmdClass);
    public Task<int> ExecuteAsync(string[] args);
}
