using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.MapsModule;

[Module(IsInternal = true)]
public class MapsModule(IMapService maps) : EvoScModule, IToggleable
{
    public async Task EnableAsync() => await maps.AddCurrentMapListAsync();

    public Task DisableAsync() => Task.CompletedTask;
}
