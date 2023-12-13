namespace EvoSC.Modules.Exceptions.ModuleDependency;

using DependencyGraph = Dictionary<string, IList<string>>;

public class DependencyCycleException(DependencyGraph remainingGraph) : DependencyException
{
    private bool FindCycle(string name, List<string> visited)
    {
        if (visited.Contains(name))
        {
            // cycle
            return true;
        }
        
        visited.Add(name);

        foreach (var dep in remainingGraph[name])
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
        foreach (var name in remainingGraph.Keys)
        {
            var cycle = new List<string>();
            
            if (FindCycle(remainingGraph.FirstOrDefault().Key, cycle))
            {
                cycle.Add(name);
                return cycle;
            }
        }

        return Array.Empty<string>();
    }

    public override string Message { get => $"Dependency cycle detected: {string.Join(" -> ", GetCycle())}"; }
}
