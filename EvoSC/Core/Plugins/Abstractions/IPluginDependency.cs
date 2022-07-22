using System;

namespace EvoSC.Core.Plugins.Abstractions;

public interface IPluginDependency
{
    public string Name { get; }
    public Version Version { get; }
    public string? ResolvedPath { get; }

    public bool ResolvePath(string pluginsDir);
}
