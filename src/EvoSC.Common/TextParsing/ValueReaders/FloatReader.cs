using System.Globalization;
using EvoSC.Common.Exceptions.Parsing;
using EvoSC.Common.Interfaces.Parsing;

namespace EvoSC.Common.TextParsing.ValueReaders;

public class FloatReader : IValueReader
{
    public IEnumerable<Type> AllowedTypes => new[]
    {
        typeof(float), 
        typeof(double)
    };
    
    public Task<object> Read(Type type, string input)
    {
        try
        {
            if (type == typeof(float))
            {
                return Task.FromResult((object)float.Parse(input, NumberStyles.Any, CultureInfo.InvariantCulture));
            }
            else if (type == typeof(double))
            {
                return Task.FromResult((object)double.Parse(input, NumberStyles.Any, CultureInfo.InvariantCulture));
            }
        }
        catch (Exception ex)
        {
            // ignore parsing exception so that we can throw ValueConversionException
        }

        throw new ValueConversionException();
    }
}
