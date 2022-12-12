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
    
    public static IInternalModuleInfo CreateFromAssembly(Assembly assembly)
    {
        var name = ValidateModuleProperty(assembly.GetCustomAttribute<ModuleIdentifierAttribute>()?.Name, "Name");
        var title = ValidateModuleProperty(assembly.GetCustomAttribute<ModuleTitleAttribute>()?.Title, "Title");
        var summary = ValidateModuleProperty(assembly.GetCustomAttribute<ModuleSummaryAttribute>()?.Summary, "Summary");
        var version = ValidateModuleProperty(assembly.GetCustomAttribute<ModuleVersionAttribute>()?.Version, "Version");
        var author = ValidateModuleProperty(assembly.GetCustomAttribute<ModuleAuthorAttribute>()?.Author, "Author");
        var dependencies = Array.Empty<IModuleInfo>();

        return new InternalModuleInfo
        {
            Name = name!,
            Title = title!,
            Summary = summary!,
            Version = version!,
            Author = author!,
            Dependencies = dependencies
        };
    }

    public static IExternalModuleInfo CreateFromDirectory(DirectoryInfo dir)
    {
        var path = Path.Combine(dir.FullName, "info.toml");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Failed to find the module's info file (info.toml) at: {path}");
        }

        var infoDocument = TomlParser.ParseFile(path);

        var name = ValidateModuleProperty(infoDocument.GetValue("info.name")?.StringValue, "Name");
        var title = ValidateModuleProperty(infoDocument.GetValue("info.name")?.StringValue, "Title");
        var summary = ValidateModuleProperty(infoDocument.GetValue("info.name")?.StringValue, "Summary");
        var versionString = ValidateModuleProperty(infoDocument.GetValue("info.name")?.StringValue, "Version");
        var author = ValidateModuleProperty(infoDocument.GetValue("info.name")?.StringValue, "Author");
        
        if (!Version.TryParse(versionString, out var version))
        {
            throw new InvalidOperationException($"Module version is in an invalid format. Cannot parse it in: {path}");
        }
        
        var dependencies = Array.Empty<IModuleInfo>();

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
