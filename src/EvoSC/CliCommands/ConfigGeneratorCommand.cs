using System.ComponentModel;
using System.Reflection;
using System.Text;
using Config.Net;
using EvoSC.CLI.Attributes;
using EvoSC.Common.Application;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database;
using EvoSC.Common.Util;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.CliCommands;

[CliCommand(Name = "genconfig", Description = "Generate a config file in provided formats.")]
[RequiredFeatures(AppFeature.Modules)]
public class ConfigGeneratorCommand
{
    private readonly IModuleManager _modules;
    
    public ConfigGeneratorCommand(IModuleManager modules)
    {
        _modules = modules;
    }
    
    public async Task ExecuteAsync(
        [Alias(Name = "-t"), Description("The output format, supported types: ENV")]
        string formatType, 
        [Alias(Name = "-f"), Description("The file to write to.")]
        string fileName
        )
    {
        if (formatType.ToUpper() != "ENV")
        {
            Console.Error.WriteLine("Invalid output format, supported types: ENV");
            return;
        }

        var output = new StringBuilder();
        
        output.AppendLine("#");
        output.AppendLine("# EvoSC# Base Config");
        output.AppendLine("#");
        output.AppendLine();
        GenerateBaseConfig(ConfigGenFormatType.ENV, output);

        output.AppendLine();
        output.AppendLine();
        
        output.AppendLine("#");
        output.AppendLine("# Module Config Options");
        output.AppendLine("#");
        output.AppendLine();
        GenerateModuleConfig(ConfigGenFormatType.ENV, output);
        
        File.WriteAllText(fileName, output.ToString());
    }

    private void GenerateBaseConfig(ConfigGenFormatType formatType, StringBuilder sb)
    {
        GeneratePropertiesRecursiveEnv(sb, typeof(IEvoScBaseConfig), null);
    }

    private void GenerateModuleConfig(ConfigGenFormatType formatType, StringBuilder sb)
    {
        var moduleAssemblies = new List<(string, string, Assembly)>();

        moduleAssemblies.AddRange(InternalModules.Modules.Select(t =>
        {
            var name = t.Assembly.GetCustomAttribute<ModuleIdentifierAttribute>();
            var version = t.Assembly.GetCustomAttribute<ModuleVersionAttribute>();

            if (name == null || version == null)
            {
                throw new InvalidOperationException(
                    $"Module type {t} does not contain the assembly attributes for module info");
            }

            return (name.Name, version.Version.ToString(), t.Assembly);
        }));
        
        var modules = _modules.LoadedModules;

        foreach (var module in modules)
        {
            foreach (var moduleAssembly in module.Assemblies)
            {
                moduleAssemblies.Add((module.ModuleInfo.Name, module.ModuleInfo.Version.ToString(), moduleAssembly));
            }
        }

        var lastModuleName = "";
        foreach (var (name, version, assembly) in moduleAssemblies)
        {
            foreach (var type in assembly.AssemblyTypesWithAttribute<SettingsAttribute>())
            {
                if (lastModuleName != name)
                {
                    sb.AppendLine();
                    sb.AppendLine($"## Module \"{name}\" v{version} ##");
                    sb.AppendLine();
                    lastModuleName = name;
                }

                GeneratePropertiesRecursiveEnv(sb, type, name);
                
                sb.AppendLine();
            }
        }
    }

    private void GeneratePropertiesRecursiveEnv(StringBuilder sb, Type type, string? name)
    {
        foreach (var property in type.GetProperties())
        {
            var optionAttr = property.GetCustomAttribute<OptionAttribute>();
            var descAttr = property.GetCustomAttribute<DescriptionAttribute>();
            var keyName = (name == null ? "" : $"{name}_") +
                          (optionAttr?.Alias != null ? $"{optionAttr.Alias}" : $"{property.Name}");

            if (property.PropertyType.IsInterface)
            {
                GeneratePropertiesRecursiveEnv(sb, property.PropertyType, keyName);
            }
            else
            {
                if (!string.IsNullOrEmpty(descAttr?.Description))
                {
                    sb.AppendLine($"# {descAttr.Description}");
                }

                sb.Append($"EVOSC_{keyName.ToUpper()}");
                sb.Append("=");
                sb.AppendLine(optionAttr?.DefaultValue?.ToString() ?? "");
                sb.AppendLine();
            }
        }
    }
}

public enum ConfigGenFormatType
{
    ENV
}
