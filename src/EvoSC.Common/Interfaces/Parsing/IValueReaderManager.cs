namespace EvoSC.Common.Interfaces.Parsing;

/// <summary>
/// Manager that collects value readers and can read multiple value types from an input string.
/// </summary>
public interface IValueReaderManager
{
    /// <summary>
    /// Add a reader to the manager.
    /// </summary>
    /// <param name="reader">Instance of the value reader.</param>
    public void AddReader(IValueReader reader);
    
    /// <summary>
    /// Get all readers of a specific type.
    /// </summary>
    /// <param name="type">The object type that a value reader can read.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public IEnumerable<IValueReader> GetReaders(Type type);
    
    /// <summary>
    /// Convert a single value to a specific type.
    /// </summary>
    /// <param name="type">The type of the object to convert to.</param>
    /// <param name="input">Input to convert from.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task<object> ConvertValueAsync(Type type, string input);

    /// <summary>
    /// Convert a single value to a specific type.
    /// </summary>
    /// <param name="input">Input to convert from.</param>
    /// <typeparam name="T">The type of the object to convert to.</typeparam>
    /// <returns></returns>
    public Task<T> ConvertValueAsync<T>(string input);
}
