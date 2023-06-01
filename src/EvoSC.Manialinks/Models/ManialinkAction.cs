using System.Reflection;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Models;

public class ManialinkAction : IManialinkAction
{
    public required string? Permission { get; set; }
    public required Type ControllerType { get; set; }
    public required MethodInfo HandlerMethod { get; set; }
    public required IMlActionParameter FirstParameter { get; set; }
}
