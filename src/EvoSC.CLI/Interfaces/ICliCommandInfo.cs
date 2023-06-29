using System.CommandLine;
using System.Reflection;
using EvoSC.CLI.Attributes;
using EvoSC.Common.Application;

namespace EvoSC.CLI.Interfaces;

public interface ICliCommandInfo
{
    public Command Command { get; }
    public Type CommandClass { get; }
    public MethodInfo HandlerMethod { get; }
    public Option[] Options { get; }
    public AppFeature[] RequiredFeatures { get; }
}
