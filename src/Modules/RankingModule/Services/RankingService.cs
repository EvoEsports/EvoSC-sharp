using System.Collections.Concurrent;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.RankingModule.Config;
using EvoSC.Modules.Official.RankingModule.Database.Models;
using EvoSC.Modules.Official.RankingModule.Interfaces;
using EvoSC.Modules.Official.RankingModule.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.RankingModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class RankingService(
    IRankingRepository repository,
    IServerClient server,
    IPlayerManagerService players,
    IMatchSettingsService matchSettings,
    IManialinkManager manialinks,
    IRankingSettings settings,
    ILogger<RankingService> logger) : IRankingService
{
    /// <summary>
    /// Account ID to the time playtime was last saved for that player.
    /// </summary>
    private readonly ConcurrentDictionary<string, DateTime> _sessions = new();

    /// <summary>
    /// Latest scores of the running map in TimeAttack, counted when the map switches.
    /// </summary>
    private ScoresEventArgs? _pendingScores;

    public void StartSession(string accountId) => _sessions[accountId] = DateTime.UtcNow;

    public async Task SendWelcomeMessageAsync(string accountId)
    {
        var rank = await GetPlayerRankAsync(accountId);

        var message = rank == null
            ? $"{PlayerRank.TextColor}You are not ranked yet, play a few maps to get a rank."
            : $"{PlayerRank.TextColor}Your rank is {PlayerRank.Highlight($"{rank.Rank}/{rank.Total}")} - {rank.Summary()}";

        await server.Chat.InfoMessageAsync(message, await players.GetOrCreatePlayerAsync(accountId));
    }

    public async Task EndSessionAsync(string accountId)
    {
        if (_sessions.TryRemove(accountId, out var since))
        {
            await SavePlaytimeAsync(accountId, since);
        }
    }

    public async Task StartSessionsForOnlinePlayersAsync()
    {
        foreach (var player in await players.GetOnlinePlayersAsync())
        {
            _sessions.TryAdd(player.AccountId, DateTime.UtcNow);
        }
    }

    public async Task SaveAllSessionsAsync()
    {
        foreach (var (accountId, since) in _sessions.ToArray())
        {
            var now = DateTime.UtcNow;

            // only reset the session if it has not been ended in the meantime
            if (_sessions.TryUpdate(accountId, now, since))
            {
                await SavePlaytimeAsync(accountId, since, now);
            }
        }
    }

    public async Task EndAllSessionsAsync()
    {
        foreach (var accountId in _sessions.Keys.ToArray())
        {
            await EndSessionAsync(accountId);
        }
    }

    public async Task HandleScoresAsync(ScoresEventArgs scores)
    {
        if (await matchSettings.GetCurrentModeAsync() == DefaultModeScriptName.TimeAttack)
        {
            // scores are sent for every round in TimeAttack, so only the latest is kept
            Interlocked.Exchange(ref _pendingScores, scores);
            return;
        }

        if (scores.Section == ModeScriptSection.EndMap)
        {
            await RecordMapResultAsync(scores);
        }
    }

    public async Task CommitMapResultAsync()
    {
        var scores = Interlocked.Exchange(ref _pendingScores, null);

        if (scores != null)
        {
            await RecordMapResultAsync(scores);
        }
    }

    private async Task RecordMapResultAsync(ScoresEventArgs scores)
    {
        foreach (var score in scores.Players.OfType<PlayerScore>())
        {
            // players with no rank did not take part, e.g. spectators
            if (score.Rank <= 0)
            {
                continue;
            }

            try
            {
                var player = await players.GetOrCreatePlayerAsync(score.AccountId);
                var won = score.Login.Equals(scores.WinnerPlayer, StringComparison.Ordinal);
                await repository.AddMapResultAsync(player.Id, won);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to record map result for player {AccountId}", score.AccountId);
            }
        }
    }

    public async Task<IReadOnlyList<PlayerRank>> GetRankingAsync()
    {
        // include time of running sessions that has not been saved yet
        await SaveAllSessionsAsync();

        var stats = await repository.GetAllStatsAsync();

        var ordered = stats
            .Where(s => s.DbPlayer != null)
            .Select(s => (Stats: s, Score: CalculateScore(s)))
            .OrderByDescending(x => x.Score)
            .ThenByDescending(x => x.Stats.PlaytimeSeconds)
            .ThenBy(x => x.Stats.PlayerId)
            .ToList();

        return ordered
            .Select((x, i) => new PlayerRank(
                i + 1,
                ordered.Count,
                x.Stats.DbPlayer!.AccountId,
                x.Stats.DbPlayer.NickName,
                x.Score,
                TimeSpan.FromSeconds(x.Stats.PlaytimeSeconds),
                x.Stats.MapsPlayed,
                x.Stats.MapsWon))
            .ToList();
    }

    public async Task<PlayerRank?> GetPlayerRankAsync(string accountId) =>
        (await GetRankingAsync()).FirstOrDefault(r => r.AccountId == accountId);

    public async Task ShowRankingAsync(IPlayer player)
    {
        var ranking = await GetRankingAsync();
        var listSize = Math.Max(1, settings.TopListSize);
        var self = ranking.FirstOrDefault(r => r.AccountId == player.AccountId);

        await manialinks.SendManialinkAsync(player, "RankingModule.Ranking", new
        {
            ranking = ranking.Take(listSize).ToList(),
            // pin the player below the list only if they did not make it into it
            footer = self is { } && self.Rank > listSize ? self : null,
            selfAccountId = player.AccountId
        });
    }

    /// <summary>
    /// Score from 0 to 100, a weighted mix of the win rate and the playtime.
    /// </summary>
    private double CalculateScore(DbPlayerRankingStats stats)
    {
        var winWeight = Math.Max(0, settings.WinRateWeight);
        var playtimeWeight = Math.Max(0, settings.PlaytimeWeight);

        if (winWeight + playtimeWeight == 0)
        {
            return 0;
        }

        var smoothedWinRate = (double)stats.MapsWon / (stats.MapsPlayed + Math.Max(0, settings.WinRateSmoothing));
        var playtimeScore = Math.Min(1, stats.PlaytimeSeconds / 3600.0 / Math.Max(1, settings.PlaytimeCapHours));

        return 100 * (winWeight * smoothedWinRate + playtimeWeight * playtimeScore) / (winWeight + playtimeWeight);
    }

    private async Task SavePlaytimeAsync(string accountId, DateTime since, DateTime? until = null)
    {
        var seconds = (long)((until ?? DateTime.UtcNow) - since).TotalSeconds;

        if (seconds <= 0)
        {
            return;
        }

        try
        {
            var player = await players.GetOrCreatePlayerAsync(accountId);
            await repository.AddPlaytimeAsync(player.Id, seconds);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save playtime for player {AccountId}", accountId);
        }
    }
}
