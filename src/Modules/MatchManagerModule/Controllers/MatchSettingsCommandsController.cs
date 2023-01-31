using System.Text;
using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Modules.Official.MatchManagerModule.Exceptions;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Modules.Official.MatchManagerModule.Permissions;

namespace EvoSC.Modules.Official.MatchManagerModule.Controllers;

[Controller]
public class MatchSettingsCommandsController : EvoScController<CommandInteractionContext>
{
    private readonly ILiveModeService _liveModeService;
    private readonly IServerClient _server;
    
    public MatchSettingsCommandsController(ILiveModeService liveModeService, IServerClient server)
    {
        _liveModeService = liveModeService;
        _server = server;
    }
    
    [ChatCommand("mode", "Change current game mode.", MatchManagerPermissions.SetLiveMode)]
    public async Task ModeAsync(string mode)
    {
        if (mode == "list")
        {
            var modes = string.Join(", ", _liveModeService.GetAvailableModes());
            await _server.SuccessMessageAsync($"Available modes: $fff{modes}");
        }
        else
        {
            try
            {
                await _liveModeService.LoadModeAsync(mode);
            }
            catch (LiveModeNotFoundException ex)
            {
                var modes = string.Join(", ", _liveModeService.GetAvailableModes());
                await _server.ErrorMessageAsync($"{ex.Message} Available modes: {modes}.");
            }
        }
    }
}
