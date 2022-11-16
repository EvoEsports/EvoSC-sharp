using System.Reflection;

namespace EvoSC.Common.Util.EnumIdentifier;

public static class EnumIdentifierUtils
{
    private static string GetIdentifierPrefix(Type enumType)
    {
        var customName = enumType.GetCustomAttribute<IdentifierAttribute>()?.Name;

        if (customName == null)
        {
            return enumType.Name;
        }

        return customName;
    }

    /// <summary>
    /// Get the ID name from an enum value as a string representation.
    /// The value can also annotate IdentifierAttribute for alternative custom names.
    /// </summary>
    /// <param name="enumValue">Enum value to get ID name from.</param>
    /// <returns></returns>
    public static string GetIdentifier(this Enum enumValue)
    {
        if (enumValue == null)
        {
            throw new InvalidOperationException("Identifier must be an enum or not null.");
        }
        
        var enumType = enumValue.GetType();
        var prefix = GetIdentifierPrefix(enumType) + ".";
        var fieldDefName = enumValue.ToString();
        var enumMember = enumType.GetMember(fieldDefName).FirstOrDefault();
        var actualName = enumMember?.GetCustomAttribute<IdentifierAttribute>()?.Name ?? fieldDefName;

        return prefix + actualName;
    }

    /// <summary>
    /// Convert an object to an enumeration value. This method also verifies that
    /// it is one and throws an exception if not.
    /// </summary>
    /// <param name="obj">Object value to convert to an enum value.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown if the conversion failed.</exception>
    public static Enum AsEnum(this object obj)
    {
        if (obj is not Enum enumValue)
        {
            throw new InvalidOperationException(
                "Value is not an enumeration value. Are you trying to pass a non-enum object to a enumeration parameter?");
        }

        return enumValue;
    }
}
