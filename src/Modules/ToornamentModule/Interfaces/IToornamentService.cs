using EvoSC.Modules.EvoEsports.ToornamentModule.Models;
using ToornamentApi.Models.Api.TournamentApi;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
public interface IToornamentService
{
    Task<DisciplineInfo?> GetDisciplineAsync(string disciplineId);
    Task<GroupInfo?> GetGroupAsync(string groupId);
    Task<List<GroupInfo>?> GetGroupsAsync(string tournamentId, string stageId);
    Task<MatchInfo?> GetMatchAsync(string matchId);
    Task<List<MatchInfo>?> GetMatchesAsync(string tournamentId, string stageId);
    Task<List<MatchGameInfo>?> GetMatchGamesAsync(string matchId);
    Task<MatchGameInfo?> GetMatchGameAsync(string matchId, int gameNumber);
    Task<MatchGameInfo?> SetMatchGameStatusAsync(string matchId, int gameNumber, MatchGameStatus status);
    Task<MatchGameInfo?> SetMatchGameResultAsync(string matchId, int gameNumber, MatchGameInfo gameInfo);
    Task<MatchGameInfo?> SetMatchGameMapAsync(string matchId, int gameNumber, string mapName);
    Task<RoundInfo?> GetRoundAsync(string roundId);
    Task<List<RoundInfo>?> GetRoundsAsync(string tournamentId, string stageId);
    Task<StageInfo?> GetStageAsync(string stageId);
    Task<List<StageInfo>?> GetStagesAsync(string tournamentId);
    Task<TournamentBasicData?> GetTournamentAsync(string tournamentId);
    Task<List<TournamentBasicData>?> GetTournamentsAsync();
}
