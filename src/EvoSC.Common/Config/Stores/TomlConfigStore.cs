using System.ComponentModel;
using System.Reflection;
using Config.Net;
using EvoSC.Common.Util;
using Tomlet;
using Tomlet.Models;

namespace EvoSC.Common.Config.Stores;

public class TomlConfigStore<TConfig> : IConfigStore where TConfig : class
{
    private TomlDocument _document;
    private readonly string _path;
    
    public TomlConfigStore(string path)
    {
        if (!File.Exists(path))
        {
            _document = CreateDefaultConfig();
            File.WriteAllText(path, _document.SerializedValue);
        }
        else
        {
            _document = TomlParser.ParseFile(path);
        }
        
        _path = Path.GetFullPath(path);
    }

    private TomlDocument CreateDefaultConfig()
    {
        var rootType = typeof(TConfig);
        var document = BuildSubDocument(TomlDocument.CreateEmpty(), rootType, "");
        
        // avoid inline writing which is more human readable
        document.ForceNoInline = false;
        
        return document;
    }
    
    private TomlDocument BuildSubDocument(TomlDocument document, Type type, string name)
    {
        foreach (var property in type.GetProperties())
        {
            if (property.PropertyType.IsInterface)
            {
                document = BuildSubDocument(document, property.PropertyType, name == "" ? property.Name : $"{name}.{property.Name}");
            }
            else
            {
                var descAttr = property.GetCustomAttribute<DescriptionAttribute>();
                var optionAttr = property.GetCustomAttribute<OptionAttribute>();

                // get property name
                var propName = optionAttr?.Alias ?? property.Name;
                propName = name == "" ? propName : $"{name}.{propName}";
                
                // get property value
                var value = TomletMain.ValueFrom(property.PropertyType,
                    optionAttr?.DefaultValue ?? property.PropertyType.GetDefaultTypeValue());

                // add description/comment if defined
                if (descAttr != null)
                {
                    value.Comments.PrecedingComment = descAttr.Description;
                }
                
                // write to document
                document.Put(propName, value);
            }
        }
        
        return document;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public string? Read(string key)
    {
        return _document.GetValue(key).StringValue;
    }

    public void Write(string key, string? value)
    {
        throw new NotImplementedException();
    }

    public bool CanRead => true;
    public bool CanWrite => false;
}
