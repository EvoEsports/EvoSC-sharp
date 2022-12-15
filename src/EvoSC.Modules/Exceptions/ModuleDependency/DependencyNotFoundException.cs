namespace EvoSC.Modules.Exceptions.ModuleDependency;

public class DependencyNotFoundException : DependencyException
{
    /// <summary>
    /// The module that has a the non-existent dependency.
    /// </summary>
    public string Dependent { get; }
    /// <summary>
    /// The dependency that was not found for the dependent.
    /// </summary>
    public string Dependency { get; }

    public DependencyNotFoundException(string dependent, string dependency) : base(
        $"The module '{dependent}' depend on '{dependency}' which doesn't exist.")
    {
        Dependent = dependent;
        Dependency = dependency;
    }
}
