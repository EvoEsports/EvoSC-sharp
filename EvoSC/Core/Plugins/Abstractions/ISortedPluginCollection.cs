using System;
using System.Collections;
using System.Collections.Generic;
using EvoSC.Core.Plugins.Exceptions;

namespace EvoSC.Core.Plugins.Abstractions;

using DependencyGraph = Dictionary<string, IList<string>>;

public abstract class ISortedPluginCollection : IPluginCollection
{
    public abstract Dictionary<string, IPluginMetaInfo> Plugins { get; }
    public abstract void Add(IPluginMetaInfo pluginMeta);
    
    /// <summary>
    /// Build an adjacency list based on plugin's dependencies.
    /// </summary>
    /// <returns></returns>
    DependencyGraph MakeDependencyGraph()
    {
        var adjList = new Dictionary<string, IList<string>>();

        foreach (var plugin in Plugins.Values)
        {
            adjList.Add(plugin.Name, new List<string>());

            foreach (var dependency in plugin.Dependencies)
            {
                adjList[plugin.Name].Add(dependency.Name);
            }
        }

        return adjList;
    }

    /// <summary>
    /// Make sure all dependencies in the dependency graph actually exists.
    /// </summary>
    /// <param name="pluginDependencies"></param>
    /// <exception cref="InvalidOperationException"></exception>
    void EnsureDependenciesExists(DependencyGraph pluginDependencies)
    {
        foreach (var (dependent, dependencies) in pluginDependencies)
        {
            foreach (var dependency in dependencies)
            {
                if (!Plugins.ContainsKey(dependency))
                {
                    throw new InvalidOperationException(
                        $"The plugin '{dependent}' depend on '{dependency}' which doesn't exist.");
                }
            }
        }
    }

    /// <summary>
    /// Sort plugins based on their dependencies so that plugins with no dependencies
    /// required comes first after each plugin is loaded.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IEnumerable<IPluginMetaInfo> SortedLoadOrder()
    {
        var pluginDependencies = MakeDependencyGraph();
        
        EnsureDependenciesExists(pluginDependencies);
        
        // sort dependencies
        var sorted = new List<IPluginMetaInfo>();
        
        while (true)
        {
            string? selected = null;
            
            foreach (var dependent in pluginDependencies)
            {
                if (dependent.Value.Count == 0)
                {
                    // zero dependencies, so just add it and remove it from all dependent plugins
                    sorted.Add(Plugins[dependent.Key]);
                    
                    foreach (var dependencies in pluginDependencies)
                    {
                        if (dependencies.Value.Contains(dependent.Key))
                        {
                            dependencies.Value.Remove(dependent.Key);
                        }
                    }

                    selected = dependent.Key;
                    break;
                }
            }
            
            if (selected != null)
            {
                pluginDependencies.Remove(selected);
            }
            else
            {
                // no more plugins that doesn't depend on anything
                break;
            }
        }

        // check if we have a dependency cycle
        if (pluginDependencies.Count > 0)
        {
            throw new DependencyCycleDetectedException(pluginDependencies);
        }

        return sorted;
    }
}
