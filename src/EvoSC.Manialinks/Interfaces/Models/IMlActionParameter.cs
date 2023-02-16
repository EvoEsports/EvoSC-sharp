using System.Reflection;

namespace EvoSC.Manialinks.Interfaces.Models;

public interface IMlActionParameter
{
    public ParameterInfo ParameterInfo { get; }
    public Type Type => ParameterInfo.ParameterType;
    public string Name => ParameterInfo.Name;
    public IMlActionParameter? NextParameter { get; set; }
}
