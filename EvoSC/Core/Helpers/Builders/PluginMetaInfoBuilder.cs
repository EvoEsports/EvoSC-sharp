using System;
using System.Collections.Generic;
using System.IO;
using EvoSC.Core.Plugins.Abstractions;
using EvoSC.Core.Plugins.Info;

namespace EvoSC.Core.Helpers.Builders;

public class PluginMetaInfoBuilder : IBuilder<IPluginMetaInfo>
{
    private string _name;
    private string _title;
    private Version _version;
    private string _summary;
    private string _author;
    private List<IPluginDependency> _dependencies;
    private List<FileInfo> _assemblyFiles;
    private bool _internal;
    private DirectoryInfo? _directory;
    private Type? _internalClass;

    public PluginMetaInfoBuilder()
    {
        _dependencies = new();
        _assemblyFiles = new();
    }

    public static PluginMetaInfoBuilder NewInternal<T>()
    {
        return new PluginMetaInfoBuilder()
            .SetInternal()
            .WithInternalClass<T>();
    }
    
    public IPluginMetaInfo Build()
    {
        return new PluginMetaInfo
        {
            Name = _name,
            Title = _title,
            Version = _version,
            Summary = _summary,
            Author = _author,
            Dependencies = _dependencies,
            AssemblyFiles = _assemblyFiles,
            IsInternal = _internal,
            InternalClass = _internalClass
        };
    }

    public PluginMetaInfoBuilder WithDirectory(DirectoryInfo dir)
    {
        _directory = dir;
        return this;
    }

    public PluginMetaInfoBuilder WithDirectory(string dir) => WithDirectory(new DirectoryInfo(dir));

    public PluginMetaInfoBuilder AddDependency(IPluginDependency dependency)
    {
        _dependencies.Add(dependency);
        return this;
    }

    public PluginMetaInfoBuilder AddAssemblyFile(FileInfo file)
    {
        _assemblyFiles.Add(file);
        return this;
    }

    public PluginMetaInfoBuilder AddAssemblyFile(string file) => AddAssemblyFile(new FileInfo(file));
    
    public PluginMetaInfoBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    
    public PluginMetaInfoBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }
    
    public PluginMetaInfoBuilder WithVersion(Version version)
    {
        _version = version;
        return this;
    }

    public PluginMetaInfoBuilder WithVersion(string version) => WithVersion(Version.Parse(version));
    
    public PluginMetaInfoBuilder WithSummary(string summary)
    {
        _summary = summary;
        return this;
    }
    
    public PluginMetaInfoBuilder WithAuthor(string author)
    {
        _author = author;
        return this;
    }
    
    public PluginMetaInfoBuilder SetInternal(bool isInternal=true)
    {
        _internal = isInternal;
        return this;
    }

    public PluginMetaInfoBuilder WithInternalClass(Type cls)
    {
        _internalClass = cls;
        return this;
    }

    public PluginMetaInfoBuilder WithInternalClass<T>() => WithInternalClass(typeof(T));
}
