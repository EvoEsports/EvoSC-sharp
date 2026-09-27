using System.Reflection;
using System.Runtime.Loader;

namespace EvoSC.Modules;

/// <summary>
/// A collectible <see cref="AssemblyLoadContext"/> for a single module. Host framework
/// assemblies resolve from the Default ALC (shared identities); module assemblies load
/// from the module's directory; export assemblies resolve through the
/// <see cref="ExportAssemblyStore"/> (owner-injected resolver).
/// </summary>
internal sealed class EvoScModuleLoadContext(string moduleAssemblyPath, Func<string, Assembly?>? exportResolver = null, bool isCollectible = true)
    : AssemblyLoadContext(Path.GetFileNameWithoutExtension(moduleAssemblyPath), isCollectible)
{
    private readonly AssemblyDependencyResolver _resolver = new(moduleAssemblyPath);
    private readonly List<string> _moduleAssemblyPaths = new();

    /// <summary>Assemblies loaded from the module's own directory.</summary>
    public IReadOnlyList<Assembly> GetModuleAssemblies()
    {
        var paths = new HashSet<string>(_moduleAssemblyPaths, StringComparer.OrdinalIgnoreCase);
        return Assemblies.Where(a => paths.Contains(a.Location)).ToArray();
    }

    /// <summary>Loads a module assembly file from disk into this context.</summary>
    public Assembly LoadModuleAssembly(string path)
    {
        var fullPath = Path.GetFullPath(path);
        var assembly = LoadFromAssemblyPath(fullPath);
        _moduleAssemblyPaths.Add(fullPath);
        return assembly;
    }

    /// <summary>Assembly names that must always resolve from the Default ALC.</summary>
    private static readonly HashSet<string> SharedAssemblyNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "EvoSC",
        "EvoSC.Common",
        "EvoSC.Commands",
        "EvoSC.Manialinks",
        "EvoSC.CLI",
        "EvoSC.Testing",
        "EvoSC.Modules",
        "SimpleInjector",
        "Tomlet",
        "Samboy063.Tomlet",
        "Config.Net",
        "ColorMinePortable",
        "GBX.NET",
        "GBX.NET.LZO",
        "linq2db",
        "linq2db.MySql",
        "linq2db.PostgreSQL",
        "linq2db.SQLite",
        "MySqlConnector",
        "FluentMigrator",
        "FluentMigrator.Runner",
        "FluentMigrator.Runner.Core",
        "FluentMigrator.Runner.Postgres",
        "FluentMigrator.Runner.MySql",
        "Humanizer",
        "Microsoft.Extensions.DependencyInjection",
        "Microsoft.Extensions.DependencyInjection.Abstractions",
        "Microsoft.Extensions.Configuration",
        "Microsoft.Extensions.Configuration.Abstractions",
        "Microsoft.Extensions.Logging",
        "Microsoft.Extensions.Logging.Abstractions",
        "Microsoft.Extensions.Options",
        "Microsoft.Extensions.Primitives"
    };

    /// <summary>Assembly name prefixes that must always resolve from the Default ALC.</summary>
    private static readonly string[] SharedAssemblyPrefixes =
    [
        "System.",
        "Microsoft.Extensions.",
        "netstandard",
        "mscorlib"
    ];

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        if (assemblyName.Name is null)
        {
            return null;
        }

        // Exports always resolve through the store so a single identity is shared by every
        // module, instead of a private copy from the module's own directory.
        if (exportResolver is not null)
        {
            var export = exportResolver(assemblyName.Name);
            if (export is not null)
            {
                return export;
            }
        }

        if (IsSharedAssembly(assemblyName.Name))
        {
            return null;
        }

        var path = _resolver.ResolveAssemblyToPath(assemblyName);
        if (path is null)
        {
            return null;
        }

        return LoadModuleAssembly(path);
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        var path = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        return path is not null ? LoadUnmanagedDllFromPath(path) : IntPtr.Zero;
    }

    internal static bool IsSharedAssembly(string assemblyName)
    {
        return SharedAssemblyNames.Contains(assemblyName) ||
               Array.Exists(SharedAssemblyPrefixes, prefix => assemblyName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }
}