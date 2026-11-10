using System.Collections.Concurrent;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces;
using EvoSC.Modules.Official.MapListModule.Models;

namespace EvoSC.Modules.Official.MapListModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MapListStateService : IMapListStateService
{
    private readonly ConcurrentDictionary<long, MapListFilter> _filters = new();
    private readonly ConcurrentDictionary<long, string> _tagMaps = new();
    private readonly ConcurrentDictionary<long, TmxSearchState> _tmxStates = new();

    public MapListFilter GetFilter(IPlayer player) => _filters.GetOrAdd(player.Id, _ => new MapListFilter());

    public void ResetFilter(IPlayer player) => _filters[player.Id] = new MapListFilter();

    public string? GetTagMapUid(IPlayer player) => _tagMaps.GetValueOrDefault(player.Id);

    public void SetTagMapUid(IPlayer player, string? mapUid)
    {
        if (mapUid == null)
        {
            _tagMaps.TryRemove(player.Id, out _);
            return;
        }

        _tagMaps[player.Id] = mapUid;
    }

    public TmxSearchState GetTmxState(IPlayer player) => _tmxStates.GetOrAdd(player.Id, _ => new TmxSearchState());
}
