using EvoSC.Common.Services.Models;

namespace EvoSC.Common.Services.Attributes;

/// <summary>
/// Classes annotated with this attribute will be automatically added to the
/// DI container. Default lifestyle is Transient unless the service is a background service.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ServiceAttribute : Attribute
{
    /// <summary>
    /// The lifestyle of this service. The default life-style is Transient.
    /// </summary>
    public ServiceLifeStyle LifeStyle { get; init; } = ServiceLifeStyle.Transient;
}
