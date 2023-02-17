using System.Reflection;

namespace EvoSC.Manialinks.Interfaces.Models;

public interface IManialinkAction
{
    public string? Permission { get; set; }
    public Type ControllerType { get; set; }
    public MethodInfo HandlerMethod { get; set; }
    public IMlActionParameter FirstParameter { get; set; }
}
