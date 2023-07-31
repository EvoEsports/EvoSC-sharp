using System.CommandLine;
using System.Reflection;
using EvoSC.CLI.Interfaces;
using EvoSC.Common.Application;

namespace EvoSC.CLI.Models;

public class CliCommandInfo : ICliCommandInfo
{
    public required Command Command { get; init; }
    public required Type CommandClass { get; init; }
    public required MethodInfo HandlerMethod { get; init; }
    public Option[] Options { get; init; }
    public AppFeature[] RequiredFeatures { get; init; }
}
