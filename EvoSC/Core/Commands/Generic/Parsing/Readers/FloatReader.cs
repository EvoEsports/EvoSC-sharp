using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        if (type == typeof(float))
        {
            return Task.FromResult((object)float.Parse(input));
        }
        else if (type == typeof(double))
        {
            return Task.FromResult((object)double.Parse(input));
        }

        throw new InvalidOperationException("Can only convert from int, uint, long or ulong.");
    }
}
