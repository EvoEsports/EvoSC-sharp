using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.EvoEsports.ToornamentModule.Models;
using ToornamentApi.Models.Api.TournamentApi;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;

public interface IMatchSettingsCreatorService
{
    Task<string> CreateMatchSettingsAsync(TournamentBasicData tournament, MatchInfo matchInfo,
        StageInfo stageInfo, GroupInfo groupInfo, RoundInfo roundInfo, IEnumerable<IMap> maps);

    Task CreateMatchSettings(string name, TrackmaniaIntegrationSettingsData settingsData,
        List<IMap> mapsToAdd);
}
