using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Modules.Official.MotdModule.Services;

namespace EvoSC.Modules.Official.MotdModule;

[Module(IsInternal = true)]
public class MotdModule : EvoScModule, IToggleable
{
    private readonly IMotdService _motdService;
    
    public MotdModule(IMotdService motdService)
    {
        _motdService = motdService;
    }
    
    public async Task EnableAsync()
    {
        await _motdService.ShowAsync();
    }

    public async Task DisableAsync()
    {
        
    }
}
