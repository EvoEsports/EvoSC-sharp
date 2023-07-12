using System.ComponentModel;
using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Modules.Official.MatchManagerModule.Permissions;

namespace EvoSC.Modules.Official.MatchManagerModule.Controllers;

[Controller]
public class MatchSettingsCommandsController : EvoScController<ICommandInteractionContext>
{
    private readonly IMatchManagerHandlerService _matchHandler;

    public MatchSettingsCommandsController(IMatchManagerHandlerService matchHandler)
    {
        _matchHandler = matchHandler;
    }

    [ChatCommand("setmode", "[Command.SetMode]", MatchManagerPermissions.SetLiveMode)]
    [CommandAlias("/mode", hide: true)]
    public Task SetModeAsync(
        [Description("[Command.SetMode.Mode]")]
        string mode
    ) => _matchHandler.SetModeAsync(mode, Context.Player);

    [ChatCommand("loadmatchsettings", "[Command.LoadMatchSettings]", MatchManagerPermissions.LoadMatchSettings)]
    [CommandAlias("/loadmatch", hide: true)]
    public Task LoadMatchSettingsAsync(
        [Description("[Command.LoadMatchSettings.Name")]
        string name
    ) => _matchHandler.LoadMatchSettingsAsync(name, Context.Player);

    [ChatCommand("scriptsetting", "[Command.ScriptSetting]", MatchManagerPermissions.SetLiveMode)]
    [CommandAlias("/ssetting", hide: true)]
    public Task SetScriptSettingAsync(string name, string value) => _matchHandler.SetScriptSettingAsync(name, value, Context.Player);
}
