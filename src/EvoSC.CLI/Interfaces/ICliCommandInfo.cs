using System.CommandLine;
using System.Reflection;
using EvoSC.Common.Application;

namespace EvoSC.CLI.Interfaces;

public interface ICliCommandInfo
{
    /// <summary>
    /// The command object for this command.
    /// </summary>
    public Command Command { get; }
    
    /// <summary>
    /// The class definition of this command.
    /// </summary>
    public Type CommandClass { get; }
    
    /// <summary>
    /// The callback method for this command.
    /// </summary>
    public MethodInfo HandlerMethod { get; }
    
    /// <summary>
    /// CLI options available for this command.
    /// </summary>
    public Option[] Options { get; }
    
    /// <summary>
    /// Application Features required to run this command.
    /// </summary>
    public AppFeature[] RequiredFeatures { get; }
}
