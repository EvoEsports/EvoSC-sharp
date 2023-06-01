using System.Reflection;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Models;

public class MlActionParameter : IMlActionParameter
{
    public required ParameterInfo ParameterInfo { get; init; }
    public IMlActionParameter? NextParameter { get; set; }
    public bool IsEntryModel { get; set; }
}
