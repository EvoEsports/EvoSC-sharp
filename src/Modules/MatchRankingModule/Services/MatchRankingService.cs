using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Models;
using EvoSC.Modules.Official.LiveRankingModule.Utils;
using EvoSC.Modules.Official.MatchRankingModule.Interfaces;
using EvoSC.Modules.Official.MatchRankingModule.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MatchRankingModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MatchRankingService : IMatchRankingService
{
    private const int ShowRows = 4;

    private readonly IManialinkManager _manialinkManager;
    private readonly IPlayerManagerService _playerManager;
    private readonly ILogger<MatchRankingService> _logger;
    private readonly IEvoScBaseConfig _config;
    private MatchRankingStore _matchRankingStore;

    public MatchRankingService(IManialinkManager manialinkManager, IPlayerManagerService playerManager,
        ILogger<MatchRankingService> logger, IEvoScBaseConfig config)
    {
        _manialinkManager = manialinkManager;
        _playerManager = playerManager;
        _logger = logger;
        _config = config;
        _matchRankingStore = new MatchRankingStore();
    }

    public Task UpdateAndShowScores(ScoresEventArgs scores)
    {
        _matchRankingStore.ConsumeScores(scores);
        return SendManialink();
    }

    public async Task SendManialink()
    {
        _logger.LogInformation("Sending manialink.");

        await _manialinkManager.SendPersistentManialinkAsync("MatchRankingModule.MatchRanking", GetWidgetData());
    }

    private dynamic GetWidgetData()
    {
        //TODO: improve so only relevant scores are mapped to save resources
        var mappedScoresPrevious = MapScoresForWidget(_matchRankingStore.GetPreviousMatchScores()).ToList().Take(ShowRows).ToList();
        var mappedScoresLatest = MapScoresForWidget(_matchRankingStore.GetLatestMatchScores()).ToList().Take(ShowRows).ToList();

        var mappedScoresExisting = mappedScoresLatest
            .Where(ranking => mappedScoresPrevious.Contains(ranking, new RankingComparer())).ToList();
        var mappedScoresNew = mappedScoresLatest.Except(mappedScoresExisting).ToList();
        
        _logger.LogInformation("Previous count: {c}", mappedScoresPrevious.Count);
        _logger.LogInformation("Latest count: {c}", mappedScoresLatest.Count);

        return new
        {
            NewScores = mappedScoresNew,
            ExistingScores = mappedScoresExisting,
            PreviousScores = mappedScoresPrevious,
            headerColor = _config.Theme.UI.HeaderBackgroundColor,
            primaryColor = _config.Theme.UI.PrimaryColor,
            logoUrl = _config.Theme.UI.LogoWhiteUrl,
            playerRowBackgroundColor = _config.Theme.UI.PlayerRowBackgroundColor
        };
    }

    private IEnumerable<LiveRankingWidgetPosition> MapScoresForWidget(ScoresEventArgs? scores)
    {
        if (scores == null)
        {
            return new List<LiveRankingWidgetPosition>();
        }

        return scores.Players.ToList()
            // .GetRange(0, ShowRows)
            .Select(score => new LiveRankingWidgetPosition(
                    score.Rank,
                    _playerManager.GetPlayerAsync(score.AccountId).Result,
                    score.MatchPoints.ToString()
                )
            )
            .ToList();
    }

    public async Task HideManialink()
    {
        _logger.LogInformation("Hiding manialink.");
        
        await _manialinkManager.HideManialinkAsync("MatchRankingModule.MatchRanking");
    }

    public Task ResetMatchData()
    {
        _logger.LogInformation("Clearing match ranking data.");
        _matchRankingStore = new MatchRankingStore();

        return Task.CompletedTask;
    }
}
