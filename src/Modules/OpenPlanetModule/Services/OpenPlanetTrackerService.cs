using System.Collections.Concurrent;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models.Dto;
using EvoSC.Modules.Official.OpenPlanetModule.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class OpenPlanetTrackerService : IOpenPlanetTrackerService
{
    private readonly ConcurrentDictionary<IPlayer, IOpenPlanetInfo> _players = new();

    public IEnumerable<IOpPlayerPair> Players =>
        _players.Select(pair => new OpPlayerPair {Player = pair.Key, OpenPlanetInfo = pair.Value});

    public void AddOrUpdatePlayer(IPlayer player, IOpenPlanetInfo playerOpInfo)
    {
        _players.AddOrUpdate(player, playerOpInfo, (_, _) => playerOpInfo);
    }

    public void RemovePlayer(IPlayer player)
    {
        _players.TryRemove(player, out _);
    }
}
