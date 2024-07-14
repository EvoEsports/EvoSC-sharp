using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.TeamInfoModule.Interfaces;

public interface ITeamInfoService
{
    public Task SendTeamInfoWidgetAsync(string playerLogin);

    public Task SendTeamInfoWidgetEveryoneAsync();

    public Task HideTeamInfoWidgetAsync(string playerLogin);

    public Task HideTeamInfoWidgetEveryoneAsync();
}
