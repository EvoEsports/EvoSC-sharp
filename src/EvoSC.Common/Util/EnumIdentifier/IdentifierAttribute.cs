namespace EvoSC.Common.Util.EnumIdentifier;

/// <summary>
/// Give an enum field a custom identifier name.
/// </summary>
[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
public class IdentifierAttribute : Attribute
{
    /// <summary>
    /// The custom ID name for this field.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Whether to not include a prefix for this identifier.
    /// </summary>
    public bool NoPrefix { get; set; }
}
