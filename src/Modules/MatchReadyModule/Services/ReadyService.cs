using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchReadyModule.Events;
using EvoSC.Modules.Official.MatchReadyModule.Events.Args;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class ReadyService(IReadyTrackerService readyTracker, IEventManager events) : IReadyService
{
    public IEnumerable<IPlayer> ReadyPlayers => readyTracker.ReadyPlayers;
    public IEnumerable<IPlayer> Players => readyTracker.Players;
    public bool Enabled => readyTracker.Enabled;
    public Task ResetAsync() => readyTracker.ClearAsync();

    public async Task AddPlayersAsync(params IPlayer[] players)
    {
        foreach (var player in players)
        {
            await readyTracker.AddPlayerAsync(player);
        }
    }

    public async Task SetPlayerReadyStatusAsync(IPlayer player, bool isReady)
    {
        if (isReady)
        {
            await readyTracker.AddReadyAsync(player);
        }
        else
        {
            await readyTracker.RemoveReadyAsync(player);
        }
        
        await events.RaiseAsync(MatchReadyEvents.PlayerReadyChanged, new PlayerReadyEventArgs
        {
            Player = player,
            IsReady = isReady
        });

        if (readyTracker.AllReady)
        {
            await events.RaiseAsync(MatchReadyEvents.AllPlayersReady, EventArgs.Empty);
        }
    }

    public async Task EnableAsync()
    {
        await readyTracker.EnableAsync();
        await events.RaiseAsync(MatchReadyEvents.Enabled, EventArgs.Empty);
    }

    public async Task DisableAsync()
    {
        await readyTracker.DisableAsync();
        await events.RaiseAsync(MatchReadyEvents.Disabled, EventArgs.Empty);
    }
}
