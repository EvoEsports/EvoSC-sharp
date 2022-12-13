using System.Reflection;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Exceptions;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Models;
using Tomlet;

namespace EvoSC.Modules.Util;

public static class ModuleInfoUtils
{
    private static T ValidateModuleProperty<T>(T? value, string name)
    {
        if (value == null)
        {
            throw new MissingModulePropertyException(name);
        }

        return value;
    }
    
    /// <summary>
    /// Create an internal module info object from an assembly.
    /// </summary>
    /// <param name="assembly">The assembly containing a module.</param>
    /// <returns></returns>
    public static IInternalModuleInfo CreateFromAssembly(Assembly assembly)
    {
        var name = ValidateModuleProperty(assembly.GetCustomAttribute<ModuleIdentifierAttribute>()?.Name, "Name");
        var title = ValidateModuleProperty(assembly.GetCustomAttribute<ModuleTitleAttribute>()?.Title, "Title");
        var summary = ValidateModuleProperty(assembly.GetCustomAttribute<ModuleSummaryAttribute>()?.Summary, "Summary");
        var version = ValidateModuleProperty(assembly.GetCustomAttribute<ModuleVersionAttribute>()?.Version, "Version");
        var author = ValidateModuleProperty(assembly.GetCustomAttribute<ModuleAuthorAttribute>()?.Author, "Author");
        var dependencies = Array.Empty<IModuleDependency>();

        return new InternalModuleInfo
        {
            Name = name!,
            Title = title!,
            Summary = summary!,
            Version = version!,
            Author = author!,
            Dependencies = dependencies,
            Assembly = assembly
        };
    }

    /// <summary>
    /// Create an external module info object from a module directory.
    /// </summary>
    /// <param name="dir">THe directory containing the module info file.</param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException">When the info file was not found.</exception>
    /// <exception cref="InvalidOperationException">When the info file has an invalid format.</exception>
    public static IExternalModuleInfo CreateFromDirectory(DirectoryInfo dir)
    {
        var path = Path.Combine(dir.FullName, "info.toml");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Failed to find the module's info file (info.toml) at: {path}");
        }

        var infoDocument = TomlParser.ParseFile(path);

        var name = ValidateModuleProperty(infoDocument.GetValue("info.name")?.StringValue, "Name");
        var title = ValidateModuleProperty(infoDocument.GetValue("info.title")?.StringValue, "Title");
        var summary = ValidateModuleProperty(infoDocument.GetValue("info.summary")?.StringValue, "Summary");
        var versionString = ValidateModuleProperty(infoDocument.GetValue("info.version")?.StringValue, "Version");
        var author = ValidateModuleProperty(infoDocument.GetValue("info.author")?.StringValue, "Author");
        
        if (!Version.TryParse(versionString, out var version))
        {
            throw new InvalidOperationException($"Module version is in an invalid format. Cannot parse it in: {path}");
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
            Name = name,
            Title = title,
            Summary = summary,
            Version = version,
            Author = author,
            Dependencies = dependencies,
            Directory = dir,
            ModuleFiles = moduleFiles
        };
    }
}
