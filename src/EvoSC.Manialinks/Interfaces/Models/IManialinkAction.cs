using System.Reflection;

namespace EvoSC.Manialinks.Interfaces.Models;

public interface IManialinkAction
{
    public string? Permission { get; init; }
    public Type ControllerType { get; init; }
    public MethodInfo HandlerMethod { get; init; }
    public IMlActionParameter FirstParameter { get; init; }
}
