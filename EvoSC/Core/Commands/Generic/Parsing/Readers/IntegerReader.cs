using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using EvoSC.Core.Commands.Generic.Exceptions;

namespace EvoSC.Core.Commands.Generic.Parsing.Readers;

public class IntegerReader : IValueReader
{
    public IEnumerable<Type> AllowedTypes => new[]
    {
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong)
    };
    
    public Task<object> Read(Type type, string input)
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
        } catch (Exception ex) {}

        throw new ValueConversionException();
    }
}
