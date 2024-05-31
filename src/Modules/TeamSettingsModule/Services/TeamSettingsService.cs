using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;

namespace EvoSC.Modules.Official.TeamSettingsModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class TeamSettingsService: ITeamSettingsService
{
    public Task ShowTeamSettingsAsync(IPlayer player)
    {
        throw new NotImplementedException();
    }
}
