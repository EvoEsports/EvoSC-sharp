using EvoSC.Commands.Interfaces;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ModuleManagerModule.ValueReaders;

namespace EvoSC.Modules.Official.ModuleManagerModule;

[Module(IsInternal = true)]
public class ModuleManagerModule(IChatCommandManager commands, IModuleManager modules) : EvoScModule, IToggleable
{
    private ModuleValueReader? _reader;

    public Task EnableAsync()
    {
        if (_reader == null)
        {
            _reader = new ModuleValueReader(modules);
            commands.ValueReader.AddReader(_reader);
        }

        return Task.CompletedTask;
    }

    public Task DisableAsync()
    {
        if (_reader != null)
        {
            commands.ValueReader.RemoveReader(_reader);
            _reader = null;
        }

        return Task.CompletedTask;
    }
}