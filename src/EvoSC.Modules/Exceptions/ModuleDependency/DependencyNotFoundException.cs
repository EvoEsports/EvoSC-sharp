namespace EvoSC.Modules.Exceptions.ModuleDependency;

public class DependencyNotFoundException(string dependent, string dependency) : DependencyException(
    $"The module '{dependent}' depend on '{dependency}' which doesn't exist.")
{
    /// <summary>
    /// The module that has a the non-existent dependency.
    /// </summary>
    public string Dependent { get; } = dependent;

    /// <summary>
    /// The dependency that was not found for the dependent.
    /// </summary>
    public string Dependency { get; } = dependency;
}
