namespace EvoSC.Common.Interfaces.Config.Mapping;

public interface ITomlMappingManager
{
    /// <summary>
    /// Add a new type mapper.
    /// </summary>
    /// <param name="mapper">An instance of the type mapper to add.</param>
    /// <typeparam name="T">The type that should be converted.</typeparam>
    public void AddMapper<T>(ITomlTypeMapper<T> mapper);

    /// <summary>
    /// Set up the default type mappers in EvoSC.
    /// </summary>
    public void SetupDefaultMappers();
}
