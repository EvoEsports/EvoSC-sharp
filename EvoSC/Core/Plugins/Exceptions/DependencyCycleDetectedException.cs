using System;
using System.Collections.Generic;
using System.Linq;

namespace EvoSC.Core.Plugins.Exceptions;

using DependencyGraph = Dictionary<string, IList<string>>;

public class DependencyCycleDetectedException : DependencyException
{
    private readonly DependencyGraph _remainingGraph;
    
    public DependencyCycleDetectedException(DependencyGraph remainingGraph)
    {
        _remainingGraph = remainingGraph;
    }

    private bool FindCycle(string name, List<string> visited)
    {
        if (visited.Contains(name))
        {
            // cycle
            return true;
        }
        
        visited.Add(name);

        foreach (var dep in _remainingGraph[name])
        {
            if (FindCycle(dep, visited))
            {
                return true;
            }
        }

        visited.Remove(name);

        return false;
    }

    /// <summary>
    /// A list representing the path of dependencies that leads to a cycle.
    /// The last element is equal to the first.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetCycle()
    {
        foreach (var name in _remainingGraph.Keys)
        {
            var cycle = new List<string>();
            
            if (FindCycle(_remainingGraph.FirstOrDefault().Key, cycle))
            {
                cycle.Add(name);
                return cycle;
            }
        }

        return Array.Empty<string>();
    }

    public override string Message { get => $"Cycle detected ({string.Join(" -> ", GetCycle())})"; }
}
