namespace EvoSC.Common.Services.Attributes;

/// <summary>
/// Classes annotated with this attribute will be automatically added to the
/// DI container.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ServiceAttribute : Attribute
{
}
