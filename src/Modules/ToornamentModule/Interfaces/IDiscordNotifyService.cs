using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
public interface IDiscordNotifyService
{
    Task NotifyMatchInfoAsync(string matchName, List<IMap> maps);
}
