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
    Transient
}
