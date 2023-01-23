using EvoSC.Common.Exceptions.Parsing;
using EvoSC.Common.Interfaces.Parsing;

namespace EvoSC.Common.TextParsing.ValueReaders;

public class IntegerReader : IValueReader
{
    public IEnumerable<Type> AllowedTypes => new[]
    {
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong)
    };
    
    public Task<object> ReadAsync(Type type, string input)
    {
        try
        {
            if (type == typeof(int))
            {
                return Task.FromResult((object)int.Parse(input));
            }
            else if (type == typeof(uint))
            {
                return Task.FromResult((object)uint.Parse(input));
            }
            else if (type == typeof(long))
            {
                return Task.FromResult((object)long.Parse(input));
            }
            else if (type == typeof(ulong))
            {
                return Task.FromResult((object)ulong.Parse(input));
            }
        }
        catch (Exception ex)
        {
            // ignore parsing exception so that we can throw ValueConversionException
        }

        throw new ValueConversionException();
    }
}
