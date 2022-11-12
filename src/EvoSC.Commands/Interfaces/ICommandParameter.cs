using System.Reflection;

namespace EvoSC.Commands.Interfaces;

public interface ICommandParameter
{
    /// <summary>
    /// The description of a command's parameter.
    /// </summary>
    public string? Description { get; }
    /// <summary>
    /// The .NET info object of the method's parameter.
    /// </summary>
    public ParameterInfo ParameterInfo { get; }
    /// <summary>
    /// Whether the parameter can be ignored or not by the user.
    /// </summary>
    public bool Optional => ParameterInfo.IsOptional;
    /// <summary>
    /// The object type of the parameter, used for parsing and input type conversion.
    /// </summary>
    public Type Type => ParameterInfo.ParameterType;
    /// <summary>
    /// The name of the parameter.
    /// </summary>
    public string Name => ParameterInfo.Name;
}
