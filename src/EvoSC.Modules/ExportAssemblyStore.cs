using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Loader;
using EvoSC.Modules.Exceptions;
using EvoSC.Modules.Util;

namespace EvoSC.Modules;

/// <summary>
/// Shared, reloadable home for module export assemblies. Exports are keyed by the providing
/// module's id so all consumers bind the same type identities. The runtime binder is answered
/// through a private name index — callers never deal in assembly names.
/// </summary>
internal sealed class ExportAssemblyStore
{
    private readonly Lock _gate = new();

    private readonly Dictionary<string, string> _knownPaths = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string> _assemblyNameToModuleId = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, ExportRegistration> _exports = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, HashSet<string>> _referencedBy = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, int> _moduleReferenceCounts = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<Guid, HashSet<string>> _instanceReferences = new();

    private readonly ConcurrentDictionary<Assembly, byte> _loadedAssemblies = new();

    /// <summary>Registers the export assembly provided by <paramref name="moduleId"/>.</summary>
    public void RegisterExportPath(string moduleId, string assemblyPath)
    {
        lock (_gate)
        {
            _knownPaths[moduleId] = assemblyPath;
            _assemblyNameToModuleId[Path.GetFileNameWithoutExtension(assemblyPath)!] = moduleId;
        }
    }

    /// <summary>
    /// True when <paramref name="assemblyPath"/> is this module's own export: unclaimed, or the
    /// exact location the export was registered from.
    /// </summary>
    public bool IsOwnExportPath(string assemblyPath)
    {
        lock (_gate)
        {
            var simpleName = Path.GetFileNameWithoutExtension(assemblyPath);
            if (!_assemblyNameToModuleId.TryGetValue(simpleName, out var moduleId))
            {
                return true;
            }

            return string.Equals(_knownPaths[moduleId], Path.GetFullPath(assemblyPath), StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>True when <paramref name="assembly"/> was loaded through this store.</summary>
    public bool IsExportAssembly(Assembly assembly) => _loadedAssemblies.ContainsKey(assembly);

    /// <summary>True when the module with id <paramref name="moduleId"/> provides an export.</summary>
    public bool HasExport(string moduleId)
    {
        lock (_gate)
        {
            return _knownPaths.ContainsKey(moduleId) || _exports.ContainsKey(moduleId);
        }
    }

    /// <summary>
    /// The already loaded export of <paramref name="moduleId"/>, without taking a reference.
    /// A module's own export holds types the module declares itself, so the module needs it for
    /// its own metadata scans; asking for a reference it already holds would leak one.
    /// </summary>
    public bool TryGetLoadedExport(string moduleId, [NotNullWhen(true)] out Assembly? assembly)
    {
        lock (_gate)
        {
            if (_exports.TryGetValue(moduleId, out var registration))
            {
                assembly = registration.Assembly;
                return true;
            }

            assembly = null;
            return false;
        }
    }

    /// <summary>
    /// Resolver used by a module's load context to answer the CLR: only registered exports are
    /// handed back, as a single shared identity.
    /// </summary>
    public Func<string, Assembly?> CreateExportResolver(Guid ownerLoadId)
        => requestedAssemblyName => ResolveForBinder(ownerLoadId, requestedAssemblyName);

    /// <summary>Loads the export of <paramref name="moduleId"/> for module instance <paramref name="ownerLoadId"/>.</summary>
    public Assembly AcquireExportForModule(Guid ownerLoadId, string moduleId)
    {
        lock (_gate)
        {
            AddInstanceReferenceLocked(ownerLoadId, moduleId);
            return AcquireOrLoadLocked(moduleId);
        }
    }

    /// <summary>Releases <paramref name="ownerLoadId"/>'s export references and unloads any export left unreferenced.</summary>
    public void ReleaseModule(Guid ownerLoadId)
    {
        HashSet<string>? referenced;
        lock (_gate)
        {
            if (!_instanceReferences.Remove(ownerLoadId, out referenced))
            {
                return;
            }

            foreach (var moduleId in referenced)
            {
                if (!_moduleReferenceCounts.TryGetValue(moduleId, out var count))
                {
                    continue;
                }

                if (count <= 1)
                {
                    _moduleReferenceCounts.Remove(moduleId);
                }
                else
                {
                    _moduleReferenceCounts[moduleId] = count - 1;
                }
            }
        }

        foreach (var moduleId in referenced)
        {
            TryUnload(moduleId);
        }
    }

    private void AddInstanceReferenceLocked(Guid ownerLoadId, string moduleId)
    {
        if (!_instanceReferences.TryGetValue(ownerLoadId, out var set))
        {
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _instanceReferences[ownerLoadId] = set;
        }

        if (set.Add(moduleId))
        {
            _moduleReferenceCounts.TryGetValue(moduleId, out var count);
            _moduleReferenceCounts[moduleId] = count + 1;
        }
    }

    /// <summary>Answers a single CLR assembly-binding request. Only exports resolve here.</summary>
    private Assembly? ResolveForBinder(Guid ownerLoadId, string requestedAssemblyName)
    {
        lock (_gate)
        {
            if (!_assemblyNameToModuleId.TryGetValue(requestedAssemblyName, out var moduleId))
            {
                return null;
            }

            if (!_exports.TryGetValue(moduleId, out var registration))
            {
                return null;
            }

            AddInstanceReferenceLocked(ownerLoadId, moduleId);
            return registration.Assembly;
        }
    }

    private Assembly AcquireOrLoadLocked(string moduleId)
    {
        if (_exports.TryGetValue(moduleId, out var existing))
        {
            return existing.Assembly;
        }

        if (!_knownPaths.TryGetValue(moduleId, out var path))
        {
            throw new EvoScModuleException(
                $"No export assembly registered for module id '{moduleId}'. A module that ships this export assembly " +
                $"must be loaded first.");
        }

        var alc = new AssemblyLoadContext($"{moduleId}:exports", isCollectible: true);
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

        var dependencyModuleIds = new List<string>();
        foreach (var referenceName in loaded.GetReferencedAssemblies().Select(static r => r.Name).OfType<string>())
        {
            if (_assemblyNameToModuleId.TryGetValue(referenceName, out var dependencyModuleId))
            {
                dependencyModuleIds.Add(dependencyModuleId);
            }
        }

        var registration = new ExportRegistration(loaded, alc, dependencyModuleIds);
        _exports[moduleId] = registration;
        _loadedAssemblies.TryAdd(loaded, 0);

        foreach (var dependency in dependencyModuleIds)
        {
            if (!_referencedBy.TryGetValue(dependency, out var by))
            {
                by = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                _referencedBy[dependency] = by;
            }

            by.Add(moduleId);
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
        if (_assemblyNameToModuleId.TryGetValue(name.Name, out var dependencyModuleId) &&
            _exports.TryGetValue(dependencyModuleId, out var otherExport))
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

    private void TryUnload(string moduleId)
    {
        ExportRegistration? registration;

        lock (_gate)
        {
            if (!_exports.TryGetValue(moduleId, out registration))
            {
                return;
            }

            if (_moduleReferenceCounts.GetValueOrDefault(moduleId) > 0)
            {
                return;
            }

            if (_referencedBy.TryGetValue(moduleId, out var referencing) && referencing.Count > 0)
            {
                return;
            }

            _exports.Remove(moduleId);
            _moduleReferenceCounts.Remove(moduleId);
            _loadedAssemblies.TryRemove(registration.Assembly, out _);
            _knownPaths.Remove(moduleId);
            _assemblyNameToModuleId.Remove(registration.Assembly.GetName().Name!);

            // Remove this export from the reverse-edge sets it contributed to.
            foreach (var dependency in registration.Dependencies)
            {
                if (_referencedBy.TryGetValue(dependency, out var by))
                {
                    by.Remove(moduleId);
                }
            }
        }

        var weak = new WeakReference(registration.LoadContext);
        registration.LoadContext.Unload();
        CollectibleLoadContext.WaitForUnload(weak);
    }

    private sealed record ExportRegistration(
        Assembly Assembly,
        AssemblyLoadContext LoadContext,
        IReadOnlyList<string> Dependencies);
}