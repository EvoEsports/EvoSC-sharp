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
        var dependencyGraph = MakeDependencyGraph();
        EnsureDependenciesExists(dependencyGraph);
        
        var sortedDependencies = new List<T>();

        while (true)
        {
            string? selected = null;

            foreach (var dependent in dependencyGraph)
            {
                if (dependent.Value.Count != 0)
                {
                    continue;
                }

                sortedDependencies.Add(_modules[dependent.Key]);
                RemoveDependent(dependencyGraph, dependent);

                selected = dependent.Key;
                break;
            }

            if (selected != null)
            {
                dependencyGraph.Remove(selected);
            }
            else
            {
                break;
            }
        }
        
        DetectCycle(dependencyGraph);
        return sortedDependencies;
    }

    private static void RemoveDependent(DependencyGraph dependencyGraph, KeyValuePair<string, IList<string>> dependent)
    {
        foreach (var dependencies in dependencyGraph)
        {
            if (dependencies.Value.Contains(dependencies.Key))
            {
                dependencies.Value.Remove(dependent.Key);
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

    private void DetectCycle(DependencyGraph dependencies)
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
