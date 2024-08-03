using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using Config.Net;
using EvoSC.CLI.Attributes;
using EvoSC.Common.Application;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Util;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.CliCommands;

[CliCommand(Name = "genconfig", Description = "Generate a config file in provided formats.")]
[RequiredFeatures(AppFeature.Modules)]
public class ConfigGeneratorCommand(IModuleManager modules)
{
    public async Task ExecuteAsync(
        [Alias(Name = "-t"), Description("The output format, supported types: ENV")]
        string formatType, 
        [Alias(Name = "-f"), Description("The file to write to.")]
        string? fileName
        )
    {
        if (formatType.ToUpper(CultureInfo.InvariantCulture) != "ENV")
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
        
        File.WriteAllText(fileName ?? ".env", output.ToString());
    }

    private void GenerateBaseConfig(ConfigGenFormatType formatType, StringBuilder sb)
    {
        GeneratePropertiesRecursiveEnv(sb, typeof(IEvoScBaseConfig), null, null);
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
        
        var loadedModules = modules.LoadedModules;

        foreach (var module in loadedModules)
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
                var prefix = type.Name[0] == 'I' ? type.Name.Substring(1) : type.Name;
                if (lastModuleName != name)
                {
                    sb.AppendLine();
                    sb.AppendLine($"## Module \"{name}\"  v{version} with Settings \"{prefix}\" ##");
                    sb.AppendLine();
                    lastModuleName = name;
                }

                GeneratePropertiesRecursiveEnv(sb, type, name, prefix);
                
                sb.AppendLine();
            }
        }
    }

    private void GeneratePropertiesRecursiveEnv(StringBuilder sb, Type type, string? name, string? prefix)
    {
        foreach (var property in type.GetProperties())
        {
            var optionAttr = property.GetCustomAttribute<OptionAttribute>();
            var descAttr = property.GetCustomAttribute<DescriptionAttribute>();
            var keyName = (name == null ? "" : $"{name}_") +
                          (optionAttr?.Alias != null ? $"{optionAttr.Alias}" : $"{property.Name}");

            if (property.PropertyType.IsInterface)
            {
                GeneratePropertiesRecursiveEnv(sb, property.PropertyType, keyName, prefix);
            }
            else
            {
                if (!string.IsNullOrEmpty(descAttr?.Description))
                {
                    sb.AppendLine($"# {descAttr.Description}");
                }

                sb.Append($"EVOSC_");

                if (!string.IsNullOrEmpty(prefix))
                {
                    sb.Append($"{prefix.ToUpper()}_");
                }
                
                sb.Append(keyName.ToUpper(CultureInfo.InvariantCulture));
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
