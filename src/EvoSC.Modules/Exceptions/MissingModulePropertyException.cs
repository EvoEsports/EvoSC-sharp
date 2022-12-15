namespace EvoSC.Modules.Exceptions;

public class MissingModulePropertyException : EvoScModuleException
{
    /// <summary>
    /// The property name that was invalid.
    /// </summary>
    public string PropertyName { get; }

    public MissingModulePropertyException(string name) : base(
        $"The module is missing meta information. The {name} property was not found.")
    {
        PropertyName = name;
    }
}
