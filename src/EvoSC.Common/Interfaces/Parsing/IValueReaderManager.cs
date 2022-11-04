namespace EvoSC.Common.Interfaces.Parsing;

public interface IValueReaderManager
{
    /// <summary>
    /// Add a reader to the manager.
    /// </summary>
    /// <param name="reader"></param>
    public void AddReader(IValueReader reader);
    /// <summary>
    /// Get all readers of a specific type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public IEnumerable<IValueReader> GetReaders(Type type);
    /// <summary>
    /// Convert a single value to a specific type.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task<object> ConvertValue(Type type, string input);
}
