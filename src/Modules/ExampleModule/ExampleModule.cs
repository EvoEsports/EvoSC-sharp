using System.Reflection;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Models;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.ExampleModule;

[Module(IsInternal = true)]
public class ExampleModule : EvoScModule, IToggleable
{
    private readonly IManialinkManager _manialinks;
    
    public ExampleModule(IManialinkManager manialinks)
    {
        _manialinks = manialinks;
    }

    public Task EnableAsync() => _manialinks.SendPersistentManialinkAsync("ExampleModule.MyManialink");

    public Task DisableAsync()
    {
        return Task.CompletedTask;
    }
}
