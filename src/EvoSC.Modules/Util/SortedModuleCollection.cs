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

    public void Add(T module)
    {
        _modules[module.Name] = module;
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
            bool found = false;
            
            foreach (var node in graph)
            {
                if (node.Value.Count > 0)
                {
                    continue;
                }
                
                sortedDependencies.Add(_modules[node.Key]);
                RemoveNodeReferences(graph, node.Key);
                found = true;
            }

            if (!found)
            {
                break;
            }
        }

        if (graph.Count > 0)
        {
            throw new DependencyCycleException(graph);
        }

        return sortedDependencies;
    }

    private static void RemoveNodeReferences(DependencyGraph graph, string nodeName)
    {
        if (graph.ContainsKey(nodeName))
        {
            graph.Remove(nodeName);
        }
        
        foreach (var node in graph)
        {
            if (node.Value.Contains(nodeName))
            {
                node.Value.Remove(nodeName);
            }
        }
    }
        
    private DependencyGraph MakeDependencyGraph()
    {
        var adjList = new Dictionary<string, IList<string>>();

        foreach (var module in _modules.Values)
        {
            adjList.Add(module.Name, new List<string>());

            foreach (var dependency in module.Dependencies)
            {
                if (_ignoredDependencies.Contains(dependency.Name))
                {
                    continue;
                }
                
                adjList[module.Name].Add(dependency.Name);
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

    private static void DetectCycle(DependencyGraph dependencies)
    {
        if (dependencies.Count > 0)
        {
            throw new DependencyCycleException(dependencies);
        }
    }

    public void SetIgnoredDependencies(IEnumerable<string> ignoredDependencies)
    {
        _ignoredDependencies.AddRange(ignoredDependencies);
    }
}
