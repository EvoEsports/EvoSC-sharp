using System.Globalization;
using EvoSC.Common.Exceptions.Parsing;
using EvoSC.Common.Interfaces.Parsing;

namespace EvoSC.Common.TextParsing.ValueReaders;

public class BooleanReader : IValueReader
{
    public IEnumerable<Type> AllowedTypes => new[] {typeof(bool)};

    private readonly Dictionary<string, bool> _lookupTable = new()
    {
        {"1", true},
        {"0", false},
        {"yes", true},
        {"no", false},
        {"true", true},
        {"false", false},
        {"on", true},
        {"off", false}
    };

    public Task<object> ReadAsync(Type type, string input)
    {
        try
        {
            if (_lookupTable.TryGetValue(input.ToLower(CultureInfo.InvariantCulture), out var value))
            {
                return Task.FromResult((object)value);
            }
        }
        catch (Exception ex)
        {
            // ignore parsing exception so that we can throw ValueConversionException
        }
        
        throw new ValueConversionException();
    }
}
