using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoSC.Core.Commands.Generic.Parsing.Readers;

public interface IValueReader
{
    public IEnumerable<Type> AllowedTypes { get; }
    public Task<object> Read(Type type, string input);
}
