namespace EvoSC.Common.Middleware.Attributes;

/// <summary>
/// Declares a class as a middleware.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class MiddlewareAttribute : Attribute
{
    
}
