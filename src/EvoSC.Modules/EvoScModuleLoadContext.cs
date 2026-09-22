using System.Reflection;
using System.Runtime.Loader;

namespace EvoSC.Modules;

/// <summary>
/// A collectible <see cref="AssemblyLoadContext"/> for loading a single module.
/// Assemblies from the host framework (EvoSC.Common, Microsoft.Extensions.*, System.*)
/// resolve from the Default ALC so all modules share the same type identity for those
/// assemblies. Module-specific assemblies load from the module's directory.
///
/// Export assemblies (either the module's own or those published by others) resolve
/// through the <see cref="ExportAssemblyStore"/> so every module shares a single
/// identity per export assembly, hosted in a dedicated collectible load context.
/// </summary>
internal sealed class EvoScModuleLoadContext(string moduleAssemblyPath, Func<string, Assembly?>? exportResolver = null)
    : AssemblyLoadContext(Path.GetFileNameWithoutExtension(moduleAssemblyPath), isCollectible: true)
{
    private readonly AssemblyDependencyResolver _resolver = new(moduleAssemblyPath);
    private readonly List<string> _moduleAssemblyPaths = new();

    /// <summary>
    /// Every assembly that was loaded from the module's own directory. Used to scope
    /// attribute scanning (services, permissions, controllers/...) to the module code
    /// only, and to release references on unload.
    /// </summary>
    public IReadOnlyList<Assembly> ModuleAssemblies
    {
        get
        {
            var paths = new HashSet<string>(_moduleAssemblyPaths, StringComparer.OrdinalIgnoreCase);
            return Assemblies.Where(a => paths.Contains(a.Location)).ToArray();
        }
    }

    /// <summary>
    /// Load an assembly file that belongs to this module from disk into this context.
    /// </summary>
    public Assembly LoadModuleAssembly(string path)
    {
        var fullPath = Path.GetFullPath(path);
        var assembly = LoadFromAssemblyPath(fullPath);
        _moduleAssemblyPaths.Add(fullPath);
        return assembly;
    }

    /// <summary>
    /// Exact assembly names that must always resolve from the Default ALC. Kept as
    /// exact matches: a bare "EvoSC" prefix would also swallow every module assembly
    /// (which are named "EvoSC.Modules.*").
    /// </summary>
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

    /// <summary>
    /// Assembly name prefixes that must always resolve from the Default ALC.
    /// </summary>
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

        // Exports always resolve through the store so a single identity is shared by
        // every module (and the module's own export dll in its directory is not loaded
        // as a private copy).
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

    /// <summary>
    /// True when the assembly name is considered "shared" and resolves from the Default ALC.
    /// </summary>
    internal static bool IsSharedAssembly(string assemblyName)
    {
        return SharedAssemblyNames.Contains(assemblyName) ||
               SharedAssemblyPrefixes.Any(prefix => assemblyName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }
}