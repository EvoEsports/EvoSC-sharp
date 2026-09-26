using System.Collections;
using EvoSC.Modules.Exceptions.ModuleDependency;
using EvoSC.Modules.Interfaces;
using DependencyGraph = System.Collections.Generic.Dictionary<string, System.Collections.Generic.IList<string>>;
namespace EvoSC.Modules.Util;

public class SortedModuleCollection<T> : IModuleCollection<T> where T : IModuleInfo
{
    private readonly Dictionary<string, T> _modules = new();
    private readonly List<string> _ignoredDependencies = new();

    /// <summary>
    /// Get a list of modules sorted by their dependencies.
    /// Complexity: O(n+m)
    /// </summary>
    public IEnumerable<T> SortedModules => GetSortedModules();

    public int Count => _modules.Count;

    public void Add(T module)
    {
        _modules[module.Id] = module;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return GetSortedModules().GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private List<T> GetSortedModules()
    {
        var graph = MakeDependencyGraph();
        EnsureDependenciesExists(graph);

        var sortedDependencies = new List<T>();

        while (true)
        {
            var resolved = graph.Where(node => node.Value.Count == 0).Select(node => node.Key).ToArray();

            if (resolved.Length == 0)
            {
                break;
            }

            foreach (var moduleId in resolved)
            {
                sortedDependencies.Add(_modules[moduleId]);
                RemoveNodeReferences(graph, moduleId);
            }
        }

        if (graph.Count > 0)
        {
            // What remains are modules that depend on each other. No order can satisfy them all,
            // so they are emitted in a stable order instead of being rejected: loading a module
            // does not require its dependencies to be loaded first, only linking and enabling do,
            // and both happen once every module of the collection is loaded.
            foreach (var moduleId in graph.Keys.OrderBy(id => id, StringComparer.Ordinal))
            {
                sortedDependencies.Add(_modules[moduleId]);
            }
        }

        return sortedDependencies;
    }

    private static void RemoveNodeReferences(DependencyGraph graph, string nodeName)
    {
        if (graph.ContainsKey(nodeName))
        {
            graph.Remove(nodeName);
        }
        
        foreach (var node in graph.Where(node => node.Value.Contains(nodeName)))
        {
            node.Value.Remove(nodeName);
        }
    }
        
    private DependencyGraph MakeDependencyGraph()
    {
        var adjList = new Dictionary<string, IList<string>>();

        foreach (var module in _modules.Values)
        {
            adjList.Add(module.Id, new List<string>());

            foreach (var dependency in module.Dependencies.Where(dependency => !_ignoredDependencies.Contains(dependency.Name)))
            {
                adjList[module.Id].Add(dependency.Name);
            }
        }

        return adjList;
    }
    
    private void EnsureDependenciesExists(DependencyGraph moduleDependencies)
    {
        foreach (var (dependent, dependencies) in moduleDependencies)
        {
            foreach (var dependency in dependencies)
            {
                if (!_modules.ContainsKey(dependency) && !_ignoredDependencies.Contains(dependency))
                {
                    throw new DependencyNotFoundException(dependent, dependency);
                }
            }
        }
    }

    public void SetIgnoredDependencies(IEnumerable<string> ignoredDependencies)
    {
        _ignoredDependencies.AddRange(ignoredDependencies);
    }
}
