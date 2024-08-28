using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchManagerModule;

[Module(IsInternal = true)]
public class MatchManagerModule(IMatchControlService matchControlService) : EvoScModule, IToggleable
{
    public Task EnableAsync()
        => matchControlService.RequestScoresAsync();

    public Task DisableAsync()
        => Task.CompletedTask;
}
