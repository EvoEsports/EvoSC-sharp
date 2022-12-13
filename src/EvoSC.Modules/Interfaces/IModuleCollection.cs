namespace EvoSC.Modules.Interfaces;

public interface IModuleCollection<T> : IEnumerable<T> where T : IModuleInfo
{
    /// <summary>
    /// Add a new module to the collection.
    /// </summary>
    /// <param name="module">The module to add.</param>
    public void Add(T module);
}
