using System.Reflection;

namespace EvoSC.Commands.Interfaces;

public interface ICommandParameter
{
    public string? Description { get; }
    public ParameterInfo ParameterInfo { get; }
    public bool Optional => ParameterInfo.IsOptional;
    public Type Type => ParameterInfo.ParameterType;
    public string Name => ParameterInfo.Name;
}
