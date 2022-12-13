namespace EvoSC.Modules.Exceptions.ModuleDependency;

public class DependencyNotFoundException : DependencyException
{
    public string Dependent { get; }
    public string Dependency { get; }

    public DependencyNotFoundException(string dependent, string dependency) : base(
        $"The module '{dependent}' depend on '{dependency}' which doesn't exist.")
    {
        Dependent = dependent;
        Dependency = dependency;
    }
}
