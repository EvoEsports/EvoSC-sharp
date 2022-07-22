using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using EvoSC.Core.Commands.Generic.Exceptions;

namespace EvoSC.Core.Commands.Generic.Parsing.Readers;

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
        } catch (Exception ex){}

        throw new ValueConversionException();
    }
}
