namespace EvoSC.Modules.Exceptions;

public class MissingModulePropertyException(string name) : EvoScModuleException(
    $"The module is missing meta information. The {name} property was not found.")
{
    /// <summary>
    /// The property name that was invalid.
    /// </summary>
    public string PropertyName { get; } = name;
}
