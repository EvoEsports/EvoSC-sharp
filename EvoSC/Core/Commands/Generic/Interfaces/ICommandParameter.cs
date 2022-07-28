using System;

namespace EvoSC.Core.Commands.Generic.Interfaces;

public interface ICommandParameter
{
    public string Name { get; }
    public string Description { get; }
    public Type ParameterType { get; }
    public bool Optional { get; }
}
