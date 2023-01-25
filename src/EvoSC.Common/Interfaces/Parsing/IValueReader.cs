namespace EvoSC.Common.Interfaces.Parsing;

public interface IValueReader
{
    /// <summary>
    /// Types that can be read from this value reader.
    /// </summary>
    public IEnumerable<Type> AllowedTypes { get; }
    
    /// <summary>
    /// Read a string value and convert it to the provided type if possible.
    /// </summary>
    /// <param name="type">The type to convert the string value to.</param>
    /// <param name="input">The value to convert</param>
    /// <returns></returns>
    public Task<object> ReadAsync(Type type, string input);
}
