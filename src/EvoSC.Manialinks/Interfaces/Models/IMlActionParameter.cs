using System.Reflection;

namespace EvoSC.Manialinks.Interfaces.Models;

/// <summary>
/// Holds information about a parameter node for a Manialink Action. This can chain together
/// a linked list. 
/// </summary>
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
    /// Reference to the next parameter in the parameter list.
    /// </summary>
    public IMlActionParameter? NextParameter { get; set; }
    
    /// <summary>
    /// Whether this parameter is a FormEntry model.
    /// </summary>
    public bool IsEntryModel { get; }
}
