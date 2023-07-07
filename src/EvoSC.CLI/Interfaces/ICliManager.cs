using System.Reflection;

namespace EvoSC.CLI.Interfaces;

public interface ICliManager
{
    /// <summary>
    /// Register CLI commands defined in an assembly with the CliCommand attribute.
    /// </summary>
    /// <param name="assembly">Assembly to search in.</param>
    /// <returns></returns>
    public ICliManager RegisterCommands(Assembly assembly);
    
    /// <summary>
    /// Register a single class as a CLI command.
    /// </summary>
    /// <param name="cmdClass">The class containing CLI command definition.</param>
    /// <returns></returns>
    public ICliManager RegisterCommand(Type cmdClass);
    
    /// <summary>
    /// Run the CLI handler.
    /// </summary>
    /// <param name="args">CLI arguments to process.</param>
    /// <returns></returns>
    public Task<int> ExecuteAsync(string[] args);
}
