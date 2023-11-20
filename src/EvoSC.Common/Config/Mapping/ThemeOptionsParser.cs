using System.Diagnostics.CodeAnalysis;
using Config.Net;
using EvoSC.Common.Themes;
using Tomlet;
using Tomlet.Models;

namespace EvoSC.Common.Config.Mapping;

public class ThemeOptionsParser : ITypeParser
{
    public bool TryParse(string? value, Type t, [UnscopedRef] out object? result)
    {
        var parser = new TomlParser();
        var doc = parser.Parse(value);
        var options = new Dictionary<string, object>();

        foreach (var entry in doc.Entries)
        {
            GetEntriesRecursive(entry.Key, entry.Value, options);
        }

        result = new DynamicThemeOptions(options);
        return true;
    }

    public string? ToRawString(object? value)
    {
        return "";
    }

    public IEnumerable<Type> SupportedTypes => new[] { typeof(DynamicThemeOptions) };

    private void GetEntriesRecursive(string name, TomlValue tomlValue, Dictionary<string, object> options)
    {
        if (tomlValue is TomlTable table)
        {
            foreach (var entry in table.Entries)
            {
                GetEntriesRecursive($"{name}.{entry.Key}", entry.Value, options);
            }
        }
        else
        {
            options[name] = tomlValue.StringValue;
        }
    }
}
