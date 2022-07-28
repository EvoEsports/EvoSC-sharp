namespace EvoSC.Core.Plugins.Exceptions;

public class DependencyNotFoundException : DependencyException
{
    public string Dependent { get; }
    public string Dependency { get; }

    public DependencyNotFoundException(string dependent, string dependency) : base(
        $"The plugin '{dependent}' depend on '{dependency}' which doesn't exist.")
    {
        Dependent = dependent;
        Dependency = dependency;
    }
}
