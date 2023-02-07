using System.ComponentModel;
using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Modules.Official.MatchManagerModule.Permissions;

namespace EvoSC.Modules.Official.MatchManagerModule.Controllers;

[Controller]
public class MatchSettingsCommandsController : EvoScController<CommandInteractionContext>
{
    private readonly IMatchManagerHandlerService _matchHandler;

    public MatchSettingsCommandsController(IMatchManagerHandlerService matchHandler)
    {
        _matchHandler = matchHandler;
    }

    [ChatCommand("setmode", "Change current game mode.", MatchManagerPermissions.SetLiveMode)]
    [CommandAlias("/mode", hide: true)]
    public Task SetModeAsync(
        [Description("The mode to change to.")]
        string mode
    ) => _matchHandler.SetModeAsync(mode, Context.Player);

    [ChatCommand("loadmatchsettings", "Load a match settings file.", MatchManagerPermissions.LoadMatchSettings)]
    [CommandAlias("/loadmatch", hide: true)]
    public Task LoadMatchSettingsAsync(
        [Description("The name of the matchsettings file, without extension.")]
        string name
    ) => _matchHandler.LoadMatchSettingsAsync(name, Context.Player);

    [ChatCommand("scriptsetting", "Set the value of a script setting.", MatchManagerPermissions.SetLiveMode)]
    [CommandAlias("/ssetting", hide: true)]
    public Task SetScriptSettingAsync(string name, string value) => _matchHandler.SetScriptSettingAsync(name, value, Context.Player);
}
