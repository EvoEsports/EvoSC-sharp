using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EvoSC.Core.Helpers;
using EvoSC.Core.Plugins.Abstractions;
using Tomlet;
using Tomlet.Exceptions;

namespace EvoSC.Core.Plugins.Info;

public class PluginMetaInfo : IPluginMetaInfo
{
    public const string MetaFileName = "info.toml";
    
    public string Name { get; init; }
    public string Title { get; init; }
    public Version Version { get; init; }
    public string Summary { get; init; }
    public string Author { get; init; }
    public DirectoryInfo? Directory { get; init; }
    public IEnumerable<IPluginDependency> Dependencies { get; init; }
    
    public IEnumerable<FileInfo> AssemblyFiles { get; init; }
    
    public bool IsInternal { get; init; }

    public static bool MetaFileExists(string pluginDir) =>
        File.Exists(Path.GetFullPath($"{pluginDir}/{MetaFileName}"));
    
    public static IPluginMetaInfo FromDirectory(string pluginDir)
    {
        if (!MetaFileExists(pluginDir))
        {
            throw new InvalidOperationException("info.json not found in the plugin directory.");
        }
        
        var metaFile = Path.GetFullPath($"{pluginDir}/{MetaFileName}");

        var metaDocument = TomlParser.ParseFile(metaFile);

        // validate
        var name = metaDocument.ValidateEntry<string>("info.name", value => Regex.IsMatch(value.StringValue, "[\\w_]+"));
        var title = metaDocument.ValidateEntry<string>("info.title", value => value.StringValue.Trim() != string.Empty);
        var version = metaDocument.ValidateEntry<string>("info.version", value => System.Version.TryParse(value.StringValue, out _));
        var author = metaDocument.ValidateEntry<string>("author", value => value.StringValue.Trim() != string.Empty);
        var summary = metaDocument.ValidateEntry<string>("summary");
        
        // plugin dir
        var pluginDirectory = new DirectoryInfo(pluginDir);
        
        // dependencies
        var dependencies = new List<IPluginDependency>();

        if (metaDocument.ContainsKey("dependencies"))
        {
            foreach (var (depName, depVersion) in metaDocument.GetSubTable("dependencies").Entries)
            {
                if (!System.Version.TryParse(depVersion.StringValue, out var ver))
                {
                    throw new ValidationException("Dependency version is in an invalid format.");
                }

                var dependency = new PluginDependency {Name = depName, Version = ver};
                
                // try to resolve the dependency path
                dependency.ResolvePath(pluginDirectory.Parent.FullName);
                
                dependencies.Add(dependency);
            }
        }
        
        // assembly files
        var asmFiles = pluginDirectory.GetFiles("*.dll", SearchOption.AllDirectories);
        
        return new PluginMetaInfo
        {
            Name = name,
            Title = title,
            Version = System.Version.Parse(version),
            Author = author,
            Summary = summary,
            
            Directory = new DirectoryInfo(pluginDir),
            
            Dependencies = dependencies,
            
            AssemblyFiles = asmFiles
        };
    }
}
