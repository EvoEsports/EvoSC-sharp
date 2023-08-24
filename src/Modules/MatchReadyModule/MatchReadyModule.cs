using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule;

[Module(IsInternal = true)]
public class MatchReadyModule : EvoScModule, IToggleable
{
    private readonly IManialinkManager _manialinks;

    public MatchReadyModule(IManialinkManager manialinks) => _manialinks = manialinks;

    public Task EnableAsync() => _manialinks.SendPersistentManialinkAsync("MatchReadyModule.ReadyWidget");

    public Task DisableAsync() => Task.CompletedTask;
}
