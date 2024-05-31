using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Manialinks.Interfaces;

namespace EvoSC.Modules.Official.TeamSettingsModule.Controllers;

[Controller]
public class TeamSettingsCommandsController(IManialinkManager manialinks, Locale locale) : EvoScController<ICommandInteractionContext>
{
    private readonly dynamic _locale = locale;
    
    [ChatCommand("teams", "[Command.TeamSettings]")]
    public async Task EditTeamSettingsAsync()
    {
        //TODO: get current settings
        
        await manialinks.SendManialinkAsync(Context.Player, "TeamSettings.EditTeamSettings",
            new
            {
                Locale = _locale
            });
    } 
}
