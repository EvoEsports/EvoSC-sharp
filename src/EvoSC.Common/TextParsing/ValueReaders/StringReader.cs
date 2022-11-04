using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Common.Interfaces.Parsing;

namespace EvoSC.Common.TextParsing.ValueReaders;

public class StringReader : IValueReader
{
    public IEnumerable<Type> AllowedTypes => new[]
    {
        typeof(string)
    };
    
    public Task<object> Read(Type type, string input)
    {
        return Task.FromResult((object)input.ToString());
    }
}
