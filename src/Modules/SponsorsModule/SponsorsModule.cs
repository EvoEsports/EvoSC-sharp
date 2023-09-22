using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.SponsorsModule.Interfaces;

namespace EvoSC.Modules.Official.SponsorsModule;

[Module(IsInternal = true)]
public class SponsorsModule : EvoScModule, IToggleable
{
    private readonly ISponsorsService _sponsorsService;

    public SponsorsModule(ISponsorsService sponsorsService)
    {
        _sponsorsService = sponsorsService;
    }

    public Task EnableAsync()
    {
        return _sponsorsService.ShowWidgetToAllSpectators();
    }

    public Task DisableAsync()
    {
        return _sponsorsService.HideWidgetFromEveryone();
    }
}
