using System.Reflection;
using EvoSC.Commands.Interfaces;

namespace EvoSC.Commands;

public class CommandParameter : ICommandParameter
{
    public required string? Description { get; init; }
    public required ParameterInfo ParameterInfo { get; init; }
}
