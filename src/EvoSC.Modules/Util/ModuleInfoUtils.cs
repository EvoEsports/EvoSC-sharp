using EvoSC.Modules.Exceptions;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Models;
using Tomlet;

namespace EvoSC.Modules.Util;

public static class ModuleInfoUtils
{
    private static T ValidateModuleProperty<T>(T? value, string name) where T : class
    {
        if (value == null)
        {
            throw new MissingModulePropertyException(name);
        }

        return value;
    }
    
    /// <summary>
    /// Create an external module info object from a module directory.
    /// </summary>
    /// <param name="dir">The directory containing the module info file.</param>
    public static IExternalModuleInfo CreateFromDirectory(DirectoryInfo dir)
    {
        var path = Path.Combine(dir.FullName, "info.toml");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Failed to find the module's info file (info.toml) at: {path}");
        }

        var infoDocument = TomlParser.ParseFile(path);

        var id = ValidateModuleProperty(infoDocument.GetValue("info.id")?.StringValue, "Id");
        var name = ValidateModuleProperty(infoDocument.GetValue("info.name")?.StringValue, "Name");
        var summary = ValidateModuleProperty(infoDocument.GetValue("info.summary")?.StringValue, "Summary");
        var versionString = ValidateModuleProperty(infoDocument.GetValue("info.version")?.StringValue, "Version");
        var author = ValidateModuleProperty(infoDocument.GetValue("info.author")?.StringValue, "Author");
        
        if (!Version.TryParse(versionString, out var version))
        {
            throw new InvalidOperationException($"Module version format is invalid. Cannot parse it in: {path}");
        }

        var dependencies = Array.Empty<IModuleDependency>().AsEnumerable();

        if (infoDocument.ContainsKey("dependencies"))
        {
            var dependencyTable = ValidateModuleProperty(infoDocument.GetSubTable("dependencies").Entries, "Dependencies");
            dependencies = dependencyTable.Select(d => new ModuleDependency
            {
                Name = d.Key, Version = Version.Parse(d.Value.StringValue)
            });
        }

        var moduleFiles = dir
            .GetFiles("*", SearchOption.AllDirectories)
            .Select(file => new ModuleFile(file));

        return new ExternalModuleInfo
        {
            Id = id,
            Name = name,
            Summary = summary,
            Version = version,
            Author = author,
            Dependencies = dependencies,
            Directory = dir,
            ModuleFiles = moduleFiles,
            IsInternal = false
        };
    }
}
