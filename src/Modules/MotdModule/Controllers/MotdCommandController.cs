using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.MotdModule.Interfaces;

namespace EvoSC.Modules.Official.MotdModule.Controllers;

[Controller]
public class MotdCommandController : EvoScController<ICommandInteractionContext>
{
    private readonly IMotdService _motdService;

    public MotdCommandController(IMotdService motdService)
    {
        _motdService = motdService;
    }

    [ChatCommand("motd", "[Command.OpenMotd]")]
    public async Task OpenMotd()
        => await _motdService.ShowAsync(Context.Player);
    
    [ChatCommand("motdsetinterval", "[Command.MotdSetFetchInterval]", "MotdPermissions.SetFetchInterval")]
    public void SetFetchIntervalAsync(int interval)
    {
        _motdService.SetInterval(interval);
    }
    
    [ChatCommand("motdseturl", "[Command.MotdSetUrl]", "MotdPermissions.SetUrl")]
    public void SetUrlAsync(string url)
    {
        _motdService.SetUrl(url);
    }
}
