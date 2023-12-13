using EvoSC.Commands.Interfaces;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ModuleManagerModule.ValueReaders;

namespace EvoSC.Modules.Official.ModuleManagerModule;

[Module(IsInternal = true)]
public class ModuleManagerModule(IChatCommandManager commands, IModuleManager modules) : EvoScModule, IToggleable
{
    public Task EnableAsync()
    {
        commands.ValueReader.AddReader(new ModuleValueReader(modules));

        return Task.CompletedTask;
    }

    public Task DisableAsync()
    {
        commands.ValueReader.RemoveReaders(typeof(IModuleLoadContext));
        
        return Task.CompletedTask;
    }
}
