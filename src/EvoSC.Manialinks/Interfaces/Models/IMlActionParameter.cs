using System.Reflection;

namespace EvoSC.Manialinks.Interfaces.Models;

public interface IMlActionParameter
{
    /// <summary>
    /// Information about the method parameter.
    /// </summary>
    public ParameterInfo ParameterInfo { get; }
    
    /// <summary>
    /// The type of the parameter.
    /// </summary>
    public Type Type => ParameterInfo.ParameterType;
    
    /// <summary>
    /// The name of the parameter.
    /// </summary>
    public string Name => ParameterInfo.Name;
    
    /// <summary>
    /// Pointer to the next parameter in the parameter list.
    /// </summary>
    public IMlActionParameter? NextParameter { get; set; }
    
    /// <summary>
    /// Whether this parameter is a FormEntry model.
    /// </summary>
    public bool IsEntryModel { get; }
}
