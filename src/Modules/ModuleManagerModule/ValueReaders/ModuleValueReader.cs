using EvoSC.Common.Exceptions.Parsing;
using EvoSC.Common.Interfaces.Parsing;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.ModuleManagerModule.ValueReaders;

public class ModuleValueReader : IValueReader
{
    private readonly IModuleManager _modules;

    public IEnumerable<Type> AllowedTypes => new[] {typeof(IModuleLoadContext)};

    public ModuleValueReader(IModuleManager modules)
    {
        _modules = modules;
    }
    
    public Task<object> ReadAsync(Type type, string input)
    {
        foreach (var module in _modules.LoadedModules)
        {
            if (module.ModuleInfo.Name.Equals(input, StringComparison.Ordinal))
            {
                return Task.FromResult((object)module);
            }
        }

        throw new ValueConversionException();
    }
}
