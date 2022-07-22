using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace EvoSC.Core.Plugins.Abstractions;

public interface IPluginMetaInfo
{
    public string Name { get; }
    public string Title { get; }
    public Version Version { get; }
    public string Summary { get; }
    public string Author { get; }
    
    public DirectoryInfo? Directory { get; }
    
    public IEnumerable<IPluginDependency> Dependencies { get; }
    
    public IEnumerable<FileInfo> AssemblyFiles { get; }
    
    public bool IsInternal { get; }
    Type? InternalClass { get; }
}
