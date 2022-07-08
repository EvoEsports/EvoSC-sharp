using System;
using EvoSC.Core.Commands.Generic.Interfaces;

namespace EvoSC.Core.Commands.Generic;

public class CommandParameter : ICommandParameter
{
    public string Name { get; }
    public string Description { get; }
    public Type ParameterType { get; }

    public CommandParameter(Type parameterType, string name, string? description=null)
    {
        Name = name;
        Description = description;
        ParameterType = parameterType;
    }
}
