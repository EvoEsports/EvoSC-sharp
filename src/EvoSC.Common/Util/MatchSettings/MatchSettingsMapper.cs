using System.Reflection;
using EvoSC.Common.Util.EnumIdentifier;
using Org.BouncyCastle.Security;

namespace EvoSC.Common.Util.MatchSettings;

public static class MatchSettingsMapper
{
    private static Dictionary<Type, string> _typeToStringMap = new()
    {
        {typeof(int), "integer"},
        {typeof(int?), "integer"},
        {typeof(uint), "integer"},
        {typeof(uint?), "integer"},
        {typeof(string), "text"},
        {typeof(bool), "boolean"},
        {typeof(bool?), "boolean"},
    };

    /// <summary>
    /// Add a custom type to the mapper. Make sure the
    /// custom type can be serialized to a string.
    /// </summary>
    /// <param name="mapFrom">The type to map.</param>
    /// <param name="mapTo">The type to map to.</param>
    public static void AddType(Type mapFrom, MatchSettingsSettingType mapTo) =>
        _typeToStringMap[mapFrom] = mapTo.GetIdentifier();
    
    /// <summary>
    /// Convert a type to a string representation for the
    /// type, which can be used to identify the type in a
    /// match settings file.
    /// </summary>
    /// <param name="t">The type to map to a string.</param>
    /// <returns></returns>
    /// <exception cref="InvalidKeyException"></exception>
    public static string ToTypeString(Type t)
    {
        if (!_typeToStringMap.ContainsKey(t))
        {
            throw new InvalidKeyException($"The type {t.Name} cannot be mapped to a string.");
        }
        
        return _typeToStringMap[t];
    }

    /// <summary>
    /// Depth-first search of a type's inheritance tree to find
    /// the first definition of a property.
    /// </summary>
    /// <param name="t">The type to search in.</param>
    /// <param name="name">The name of the property to look for.</param>
    /// <returns></returns>
    public static PropertyInfo? FindBasePropertyDefinition(Type t, string name)
    {
        var properties = t.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.Name.Equals(name, StringComparison.Ordinal))
            {
                if (t.BaseType == null)
                {
                    return property;
                }
                
                var propertyDefinition = FindBasePropertyDefinition(t.BaseType, name);

                if (propertyDefinition == null)
                {
                    return property;
                }

                return propertyDefinition;
            }
        }

        return null;
    }
}
