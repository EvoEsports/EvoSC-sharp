using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Modules.Official.MotdModule.Interfaces;

namespace EvoSC.Modules.Official.MotdModule.Controllers;

[Controller]
public class MotdCommandController : EvoScController<ICommandInteractionContext>
{
    private readonly IMotdService _motdService;

    public MotdCommandController(IMotdService motdService, IContextService context)
    {
        _motdService = motdService;
    }

    [ChatCommand("motdsetlocal", "[Command.SetMotdLocal]", MotdPermissions.SetLocal)]
    public void SetMotdLocal(string local)
    {
        if (local == "true")
        {
            _motdService.SetMotdSource(true, Context.Player);
        }
        else if (local == "false")
        {
            _motdService.SetMotdSource(false, Context.Player);
        }
    } 

    [ChatCommand("motdedit", "[Command.EditMotd]", MotdPermissions.OpenMotdEdit)]
    public async Task OpenEditMotdAsync()
        => await _motdService.ShowEditAsync(Context.Player);

    [ChatCommand("motd", "[Command.OpenMotd]")]
    public async Task OpenMotdAsync()
        => await _motdService.ShowAsync(Context.Player, true);
    
    [ChatCommand("motdsetinterval", "[Command.MotdSetFetchInterval]", MotdPermissions.SetFetchInterval)]
    public void SetFetchInterval(int interval)
    {
        _motdService.SetInterval(interval, Context.Player);
    }
    
    [ChatCommand("motdseturl", "[Command.MotdSetUrl]", MotdPermissions.SetUrl)]
    public void SetUrl(string url)
    {
        _motdService.SetUrl(url, Context.Player);
    }
}
