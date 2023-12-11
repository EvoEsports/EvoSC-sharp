using EvoSC.Common.Interfaces.Config.Mapping;
using EvoSC.Common.Themes;
using Tomlet;
using Tomlet.Models;

namespace EvoSC.Common.Config.Mapping.Toml;

public class ThemeConfigOptionsMapper : ITomlTypeMapper<DynamicThemeOptions>
{
    public TomlValue Serialize(DynamicThemeOptions? typeValue)
    {
        var doc = TomlDocument.CreateEmpty();

        foreach (var option in typeValue)
        {
            BuildTomlDocument(doc, option.Key.Split('.'), option.Value);
        }
        
        return doc;
    }

    public DynamicThemeOptions Deserialize(TomlValue tomlValue)
    {
        var doc = tomlValue as TomlDocument;

        if (doc == null)
        {
            throw new InvalidOperationException("Value is not a document.");
        }

        var options = new DynamicThemeOptions();
        
        foreach (var entry in doc.Entries)
        {
            BuildOptionsObject(options, entry.Key, entry.Value);
        }

        return options;
    }

    private void BuildTomlDocument(TomlDocument doc, IEnumerable<string> optionParts, object value)
    {
        var parts = optionParts as string[] ?? optionParts.ToArray();

        if (parts.Length == 1)
        {
            var tomlValue = TomletMain.ValueFrom(value);
            doc.Entries[parts.First()] = tomlValue;
            return;
        }
        
        foreach (var part in parts)
        {
            if (doc.ContainsKey(part))
            {
                BuildTomlDocument((TomlDocument)doc.Entries[part], parts.Skip(1), value);
            }
            else
            {
                var newDoc = TomlDocument.CreateEmpty();
                doc.Entries[part] = newDoc;
                BuildTomlDocument(newDoc, parts.Skip(1), value);
            }
        }
    }

    private void BuildOptionsObject(DynamicThemeOptions options, string key, TomlValue tomlValue)
    {
        var doc = tomlValue as TomlDocument;

        if (doc == null)
        {
            options[key] = tomlValue.StringValue;
        }

        foreach (var entry in doc.Entries)
        {
            BuildOptionsObject(options, $"{key}.{entry.Key}", entry.Value);
        }
    }
}
