using ToornamentApi.Models.Api.TournamentApi;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;

public interface IWhitelistService
{
    Task WhitelistPlayers(OpponentInfo[] opponents);
    Task WhitelistSpectators();
    Task ForcePlayerIntoSpectate(string login);
    Task KickNonWhitelistedPlayers();
}
