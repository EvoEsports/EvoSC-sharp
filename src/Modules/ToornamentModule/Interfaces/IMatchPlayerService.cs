using EvoSC.Common.Interfaces.Models;
using ToornamentApi.Models.Api.TournamentApi;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;

public interface IMatchPlayerService
{
    Task<List<IPlayer>> GetPlayersFromOpponents(OpponentInfo[] opponents);
}
