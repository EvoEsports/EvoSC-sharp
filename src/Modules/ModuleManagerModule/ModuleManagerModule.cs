using EvoSC.Commands.Interfaces;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ModuleManagerModule.ValueReaders;

namespace EvoSC.Modules.Official.ModuleManagerModule;

[Module(IsInternal = true)]
public class ModuleManagerModule : EvoScModule, IToggleable
{
    private readonly IChatCommandManager _commands;
    private readonly IModuleManager _modules;
    
    public ModuleManagerModule(IChatCommandManager commands, IModuleManager modules)
    {
        _commands = commands;
        _modules = modules;
    }

    public Task EnableAsync()
    {
        _commands.ValueReader.AddReader(new ModuleValueReader(_modules));

        return Task.CompletedTask;
    }

    public Task DisableAsync()
    {
        _commands.ValueReader.RemoveReaders(typeof(IModuleLoadContext));
        
        return Task.CompletedTask;
    }
}
