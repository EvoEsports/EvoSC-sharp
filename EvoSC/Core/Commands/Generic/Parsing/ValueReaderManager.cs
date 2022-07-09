using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Core.Commands.Generic.Exceptions;
using EvoSC.Core.Commands.Generic.Parsing.Readers;

namespace EvoSC.Core.Commands.Generic.Parsing;

public class ValueReaderManager
{
    private Dictionary<Type, List<IValueReader>> _readers = new();

    /// <summary>
    /// Add a reader to the manager.
    /// </summary>
    /// <param name="reader"></param>
    public void AddReader(IValueReader reader)
    {
        foreach (var type in reader.AllowedTypes)
        {
            if (!_readers.ContainsKey(type))
            {
                _readers.Add(type, new List<IValueReader>());
            }
        
            _readers[type].Add(reader);
        }
    }

    /// <summary>
    /// Get all readers of a specific type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public IEnumerable<IValueReader> GetReaders(Type type)
    {
        if (_readers.TryGetValue(type, out var readers))
        {
            return readers;
        }

        throw new ArgumentException($"Readers of type {type.Name} doesn't exist");
    }

    /// <summary>
    /// Convert a single value to a specific type.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<object> ConvertValue(Type type, string input)
    {
        var readers = GetReaders(type);

        foreach (var reader in readers)
        {
            try
            {
                return await reader.Read(type, input);
            }
            catch (ValueConversionException)
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        throw new FormatException();
    }
}
