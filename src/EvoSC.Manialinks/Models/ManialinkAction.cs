using System.Reflection;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Models;

public class ManialinkAction : IManialinkAction
{
    public required string? Permission { get; init; }
    public required Type ControllerType { get; init; }
    public required MethodInfo HandlerMethod { get; init; }
    public required IMlActionParameter FirstParameter { get; init; }
}
