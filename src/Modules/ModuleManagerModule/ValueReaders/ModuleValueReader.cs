using EvoSC.Common.Exceptions.Parsing;
using EvoSC.Common.Interfaces.Parsing;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.ModuleManagerModule.ValueReaders;

public class ModuleValueReader(IModuleManager modules) : IValueReader
{
    public IEnumerable<Type> AllowedTypes { get; } = [typeof(IModuleLoadContext)];

    public Task<object> ReadAsync(Type type, string input)
    {
        var module = modules.GetLoadedModules()
            .FirstOrDefault(m => m.ModuleInfo.Id.Equals(input, StringComparison.Ordinal))
            ?? throw new ValueConversionException($"No module with the identifier '{input}' is loaded.");

        return Task.FromResult((object)module);
    }
}