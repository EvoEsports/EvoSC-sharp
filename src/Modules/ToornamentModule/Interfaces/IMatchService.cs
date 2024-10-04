using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Remote.EventArgsModels;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;

public interface IMatchService
{
    public Task ShowSetupScreenAsync(IPlayer player, string selectedTournamentId, string selectedStageId);

    public Task SetupServerAsync(IPlayer player, string tournamentId, string stageId, string matchId);

    public Task FinishServerSetupAsync();

    public Task StartMatchAsync();

    public Task EndMatchAsync(ScoresEventArgs timeline);

    public Task<bool> SetServerNameAsync(string name);

    public Task SetMatchGameMapAsync();
}
