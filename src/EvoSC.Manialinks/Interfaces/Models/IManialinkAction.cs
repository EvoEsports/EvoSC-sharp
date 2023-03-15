using System.Reflection;

namespace EvoSC.Manialinks.Interfaces.Models;

public interface IManialinkAction
{
    public string? Permission { get; }
    public Type ControllerType { get; }
    public MethodInfo HandlerMethod { get; }
    public IMlActionParameter FirstParameter { get; }
}
