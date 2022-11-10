namespace EvoSC.Common.Events.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class EventIdentifierAttribute : Attribute
{
    public required string Name { get; init; }
}
