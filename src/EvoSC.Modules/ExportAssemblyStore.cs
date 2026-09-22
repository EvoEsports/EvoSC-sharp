using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Loader;
using EvoSC.Modules.Exceptions;

namespace EvoSC.Modules;

/// <summary>
/// Loads every module's export assembly into its own dedicated collectible
/// <see cref="AssemblyLoadContext"/> so that:
///
/// - All modules that reference a given export resolve the same assembly instance
///   (and therefore the same type identities), because they all resolve through this store.
/// - An export assembly can be unloaded and later reloaded once no module that still
///   references it remains loaded — i.e. exports are as reloadable as the modules
///   themselves.
///
/// Exports are reference-counted. Every module that references an export (its own or
/// another module's) adds a reference; releasing a module decrements each of its
/// references. An export is only unloaded when no live module reference remains and no
/// other still-loaded export assembly references it.
/// </summary>
internal sealed class ExportAssemblyStore
{
    private readonly Lock _gate = new();

    /// <summary>known export simple name -> path it was registered from.</summary>
    private readonly Dictionary<string, string> _knownPaths = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>export simple name -> registration (assembly + owning ALC).</summary>
    private readonly Dictionary<string, ExportRegistration> _exports = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>export simple name -> set of export names that reference it.</summary>
    private readonly Dictionary<string, HashSet<string>> _referencedBy = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>export simple name -> number of live modules referencing it.</summary>
    private readonly Dictionary<string, int> _moduleReferenceCounts = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>live module load id -> set of export simple names it references.</summary>
    private readonly Dictionary<Guid, HashSet<string>> _moduleReferences = new();

    private readonly ConcurrentDictionary<Assembly, byte> _loadedAssemblies = new();

    /// <summary>
    /// Registers the location of an export assembly so it can be loaded on demand,
    /// even by a module that does not itself ship the export file.
    /// </summary>
    public void RegisterExportPath(string simpleName, string assemblyPath)
    {
        lock (_gate)
        {
            _knownPaths[simpleName] = assemblyPath;
        }
    }

    /// <summary>
    /// True when <paramref name="assembly"/> is an export assembly loaded through this store.
    /// </summary>
    public bool IsExportAssembly(Assembly assembly) => _loadedAssemblies.ContainsKey(assembly);

    /// <summary>
    /// Acquires (loading on first use) the export assembly and records a reference on behalf
    /// of <paramref name="ownerLoadId"/>. Multiple acquires by the same module are idempotent.
    /// </summary>
    public Assembly AcquireExportForModule(Guid ownerLoadId, string simpleName)
    {
        lock (_gate)
        {
            AddModuleReferenceLocked(ownerLoadId, simpleName);
            return AcquireOrLoadLocked(simpleName);
        }
    }

    /// <summary>
    /// Resolves an export assembly that is expected to already be loaded. Records a module
    /// reference when found. Returns null when the export is unknown.
    /// </summary>
    public Assembly? ResolveExportForModule(Guid ownerLoadId, string simpleName)
    {
        lock (_gate)
        {
            if (!_exports.TryGetValue(simpleName, out var registration))
            {
                return null;
            }

            AddModuleReferenceLocked(ownerLoadId, simpleName);
            return registration.Assembly;
        }
    }

    /// <summary>
    /// Releases every export reference owned by <paramref name="ownerLoadId"/> and unloads
    /// any export that has become unreferenced.
    /// </summary>
    public void ReleaseModule(Guid ownerLoadId)
    {
        HashSet<string>? names;
        lock (_gate)
        {
            if (!_moduleReferences.Remove(ownerLoadId, out names))
            {
                return;
            }

            foreach (var name in names)
            {
                if (!_moduleReferenceCounts.TryGetValue(name, out var count))
                {
                    continue;
                }

                if (count <= 1)
                {
                    _moduleReferenceCounts.Remove(name);
                }
                else
                {
                    _moduleReferenceCounts[name] = count - 1;
                }
            }
        }

        foreach (var name in names)
        {
            TryUnload(name);
        }
    }

    private void AddModuleReferenceLocked(Guid ownerLoadId, string simpleName)
    {
        if (!_moduleReferences.TryGetValue(ownerLoadId, out var set))
        {
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _moduleReferences[ownerLoadId] = set;
        }

        if (set.Add(simpleName))
        {
            _moduleReferenceCounts.TryGetValue(simpleName, out var count);
            _moduleReferenceCounts[simpleName] = count + 1;
        }
    }

    private Assembly AcquireOrLoadLocked(string simpleName)
    {
        if (_exports.TryGetValue(simpleName, out var existing))
        {
            return existing.Assembly;
        }

        if (!_knownPaths.TryGetValue(simpleName, out var path))
        {
            throw new EvoScModuleException(
                $"No export assembly registered for '{simpleName}'. A module that ships an export assembly " +
                $"named '{simpleName}.Exports' must be loaded first.");
        }

        var alc = new AssemblyLoadContext($"{simpleName}:exports", isCollectible: true);
        var resolver = new AssemblyDependencyResolver(path);

        Assembly loaded;
        try
        {
            alc.Resolving += (_, name) => ResolveExportDependency(name, resolver, alc);
            loaded = alc.LoadFromAssemblyPath(path);
        }
        catch
        {
            alc.Unload();
            throw;
        }

        // Record cross-export edges so we never unload an export that another
        // still-loaded export assembly references.
        var dependencyNames = new List<string>();
        foreach (var referenceName in loaded.GetReferencedAssemblies())
        {
            if (referenceName.Name is not null && _exports.ContainsKey(referenceName.Name))
            {
                dependencyNames.Add(referenceName.Name);
            }
        }

        var registration = new ExportRegistration(loaded, alc, dependencyNames);
        _exports[simpleName] = registration;
        _loadedAssemblies.TryAdd(loaded, 0);

        foreach (var dependency in dependencyNames)
        {
            if (!_referencedBy.TryGetValue(dependency, out var by))
            {
                by = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                _referencedBy[dependency] = by;
            }

            by.Add(simpleName);
        }

        return loaded;
    }

    private Assembly? ResolveExportDependency(AssemblyName name,
        AssemblyDependencyResolver resolver, AssemblyLoadContext callerAlc)
    {
        if (name.Name is null)
        {
            return null;
        }

        // Another export? Resolve through the store (same identity everywhere).
        if (_exports.TryGetValue(name.Name, out var otherExport))
        {
            return otherExport.Assembly;
        }

        // Shared framework assemblies resolve from the default context.
        if (EvoScModuleLoadContext.IsSharedAssembly(name.Name))
        {
            return null;
        }

        var path = resolver.ResolveAssemblyToPath(name);
        return path is not null ? callerAlc.LoadFromAssemblyPath(path) : null;
    }

    private void TryUnload(string simpleName)
    {
        ExportRegistration? registration;

        lock (_gate)
        {
            if (!_exports.TryGetValue(simpleName, out registration))
            {
                return;
            }

            if (_moduleReferenceCounts.GetValueOrDefault(simpleName) > 0)
            {
                return;
            }

            if (_referencedBy.TryGetValue(simpleName, out var referencing) && referencing.Count > 0)
            {
                return;
            }

            _exports.Remove(simpleName);
            _moduleReferenceCounts.Remove(simpleName);
            _loadedAssemblies.TryRemove(registration.Assembly, out _);

            // Remove this export from the reverse-edge sets it contributed to.
            foreach (var dependency in registration.Dependencies)
            {
                if (_referencedBy.TryGetValue(dependency, out var by))
                {
                    by.Remove(simpleName);
                }
            }
        }

        var weak = new WeakReference(registration.LoadContext);
        registration.LoadContext.Unload();
        ForceCollection(weak);
    }

    /// <summary>
    /// Forces garbage collection until the export's load context (referenced only through
    /// <paramref name="weak"/>) has been collected. Never inline: the JIT must not keep a
    /// strong reference to the load context in this frame during the pass.
    /// </summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static void ForceCollection(WeakReference? weak)
    {
        if (weak is null)
        {
            return;
        }

        for (var i = 0; i < 10 && weak.IsAlive; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    private sealed record ExportRegistration(
        Assembly Assembly,
        AssemblyLoadContext LoadContext,
        IReadOnlyList<string> Dependencies);
}