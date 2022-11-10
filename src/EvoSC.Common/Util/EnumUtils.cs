using System.Reflection;
using EvoSC.Common.Events.Attributes;

namespace EvoSC.Common.Util;

public static class EnumUtils
{
    /// <summary>
    /// Get the event name from an enum value.
    /// The value can also annotate EventIdentifierAttribute for alternative custom names.
    /// </summary>
    /// <param name="enumValue">Enum value to get event name from.</param>
    /// <returns></returns>
    public static string GetEventIdentifier(this Enum enumValue)
    {
        if (enumValue == null)
        {
            throw new InvalidOperationException("Event identifier must be an enum or not null.");
        }
        
        var name = enumValue.ToString();
        var member = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();

        return member?.GetCustomAttribute<EventIdentifierAttribute>()?.Name ?? name;
    }
}
