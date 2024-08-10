using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.Player;

[Module(IsInternal = true)]
public class PlayerModule(IPlayerCacheService playerCacheService) : EvoScModule, IToggleable
{
    public Task EnableAsync() => playerCacheService.UpdatePlayerListAsync();

    public Task DisableAsync() => Task.CompletedTask;
}
