using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;

namespace EvoSC.Modules.Official.TeamSettingsModule.Controllers;

[Controller]
public class TeamSettingsController(ITeamSettingsService teamSettingsService, Locale locale) : ManialinkController
{
    private readonly dynamic _locale = locale;

    public async Task UpdateSettingsFromRequest()
    {
        
    }
}
