namespace EvoSC.Common.Util.EnumIdentifier;

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = true)]
public class AliasAttribute : Attribute
{
    /// <summary>
    /// The name of the alias.
    /// </summary>
    public string Name { get; set; }
}
