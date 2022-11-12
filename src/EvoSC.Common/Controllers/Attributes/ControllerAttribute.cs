using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Controllers.Attributes;

/// <summary>
/// Defines the annotated class as a controller class and that it should be registered.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ControllerAttribute : Attribute
{
    
}
