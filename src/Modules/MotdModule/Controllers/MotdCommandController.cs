using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.MotdModule.Interfaces;

namespace EvoSC.Modules.Official.MotdModule.Controllers;

[Controller]
public class MotdCommandController(IMotdService motdService) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("motdsetlocal", "[Command.SetMotdLocal]", MotdPermissions.SetLocal)]
    public void SetMotdLocal(string local)
    {
        if (local == "true")
        {
            motdService.SetMotdSource(true, Context.Player);
        }
        else if (local == "false")
        {
            motdService.SetMotdSource(false, Context.Player);
        }
    } 

    [ChatCommand("motdedit", "[Command.EditMotd]", MotdPermissions.EditMotd)]
    public async Task OpenEditMotdAsync()
        => await motdService.ShowEditAsync(Context.Player);

    [ChatCommand("motd", "[Command.OpenMotd]")]
    public async Task OpenMotdAsync()
        => await motdService.ShowAsync(Context.Player, true);
    
    [ChatCommand("motdsetinterval", "[Command.MotdSetFetchInterval]", MotdPermissions.SetFetchInterval)]
    public void SetFetchInterval(int interval)
    {
        motdService.SetInterval(interval, Context.Player);
    }
    
    [ChatCommand("motdseturl", "[Command.MotdSetUrl]", MotdPermissions.SetUrl)]
    public void SetUrl(string url)
    {
        motdService.SetUrl(url, Context.Player);
    }
}
