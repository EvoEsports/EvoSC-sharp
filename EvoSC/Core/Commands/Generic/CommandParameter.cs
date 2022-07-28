using System;
using EvoSC.Core.Commands.Generic.Interfaces;

namespace EvoSC.Core.Commands.Generic;

public class CommandParameter : ICommandParameter
{
    public string Name { get; }
    public string Description { get; }
    public Type ParameterType { get; }
    public bool Optional { get; }

    public CommandParameter(Type parameterType, string name, bool optional, string? description=null)
    {
        Name = name;
        Description = description;
        ParameterType = parameterType;
        Optional = optional;
    }
}
