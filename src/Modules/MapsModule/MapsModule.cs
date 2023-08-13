using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.Maps;

[Module(IsInternal = true)]
public class MapsModule : EvoScModule, IToggleable
{
    private readonly IMapService _maps;

    public MapsModule(IMapService maps)
    {
        _maps = maps;
    }

    public async Task EnableAsync() => await _maps.AddCurrentMapListAsync();

    public Task DisableAsync() => Task.CompletedTask;
}
