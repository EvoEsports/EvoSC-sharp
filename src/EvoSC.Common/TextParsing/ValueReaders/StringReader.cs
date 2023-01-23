using EvoSC.Common.Interfaces.Parsing;

namespace EvoSC.Common.TextParsing.ValueReaders;

public class StringReader : IValueReader
{
    public IEnumerable<Type> AllowedTypes => new[]
    {
        typeof(string)
    };
    
    public Task<object> ReadAsync(Type type, string input)
    {
        return Task.FromResult((object)input);
    }
}
