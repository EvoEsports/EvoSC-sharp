namespace EvoSC.Modules;

public enum ServiceLifeStyle
{
    /// <summary>
    /// The service is instantiated once. Be aware of multi-threading issues when using this life-style.
    /// </summary>
    Singleton,
    /// <summary>
    /// The service is instantiated every time the constructor is called on an object. Transient services are mostly
    /// thread safe, unless they depend upon a singleton that isn't.
    /// </summary>
    Transient,
    /// <summary>
    /// The service is instantiated once within a scope. A scope lives for the entirety of an event.
    /// For example when calling a command, if the service is requested multiple times,
    /// the first instance created will be returned. This allows keeping of states within the entire
    /// scope/lifetime of a request.
    /// </summary>
    Scoped
}
