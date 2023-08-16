using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.LiveRankingModule.Models;

internal class LiveRankingStore
{
    private List<LiveRankingPosition> _curLiveRanking = new();
    private readonly ILogger<LiveRankingStore> _logger;
    private readonly IPlayerManagerService _playerManager;
    private MatchInfo _matchInfo = new();

    internal LiveRankingStore(ILogger<LiveRankingStore> logger, IPlayerManagerService playerManager)
    {
        _logger = logger;
        _playerManager = playerManager;
        _logger.LogInformation("==================================================================");
        _logger.LogInformation("==================================================================");
        _logger.LogInformation("==================================================================");
        _logger.LogInformation("Instantiated LiveRankingStore");
        _logger.LogInformation("==================================================================");
        _logger.LogInformation("==================================================================");
        _logger.LogInformation("==================================================================");
    }

    internal async Task ResetLiveRankingsAsync()
    {
        _curLiveRanking = new();
        var onlinePlayers = await _playerManager.GetOnlinePlayersAsync();
        foreach (var player in onlinePlayers)
        {
            if (player.State == PlayerState.Playing)
            {
                _curLiveRanking.Add(new LiveRankingPosition(player.AccountId, 0 , -1, false, false));
            }
        }
    }
    
    internal bool RegisterTime(string accountId, int cpIndex, int cpTime, bool isFinish)
    {
        var index = _curLiveRanking.FindIndex(x => x.accountId == accountId);
        var liveRankingPosition = new LiveRankingPosition(accountId, cpTime, cpIndex, false, isFinish);
        if (index != -1)
        {
            _curLiveRanking[index] = liveRankingPosition;
        }
        // This case should never happen, but trying to avoid IndexOutOfRangeException
        else
        {
            _curLiveRanking.Add(liveRankingPosition);
        }

        return true;
    }

    internal bool RegisterPlayerGiveUp(string accountId)
    {
        var index = _curLiveRanking.FindIndex(x => x.accountId == accountId);
        if (index != -1)
        {
            var liveRanking = _curLiveRanking[index];
            _curLiveRanking[index] = new LiveRankingPosition(accountId, liveRanking.cpTime, liveRanking.cpIndex, true, false);
            return true;
        }
        else
        {
            _curLiveRanking.Add(new LiveRankingPosition(accountId, 0, 0, true, false));
            return true;
        }
    }

    internal List<LiveRankingPosition> GetCurrentLiveRanking()
    {
        return _curLiveRanking;
    }
    /// <summary>
    /// This sorts the live ranking based on the following criteria:
    /// - DNFd players should always be at the bottom
    /// - Players with a higher cpIndex should always be at the top
    /// - Players with the faster CP time at the same CP index should be in a higher position
    /// </summary>
    internal void SortLiveRanking()
    {
        _curLiveRanking = _curLiveRanking
            .OrderBy(a => a.isDNF)
            .ThenByDescending(a => a.cpIndex)
            .ThenBy(a => a.cpTime)
            .ToList();
    }

    internal async Task<List<ExpandedLiveRankingPosition>> GetFullLiveRankingAsync()
    {
        List<ExpandedLiveRankingPosition> expandedLiveRanking = new();
        foreach (var rank in _curLiveRanking)
        {
            var player = await _playerManager.GetOnlinePlayerAsync(rank.accountId);
            expandedLiveRanking.Add(new ExpandedLiveRankingPosition(player,
                rank.cpTime,
                rank.cpIndex,
                rank.isDNF,
                rank.isFinish));
        }

        return expandedLiveRanking;
    }

    internal void SetCurrentMap(string name)
    {
        _matchInfo.MapName = name;
    }

    internal void SetWorldRecord(string wrHolder, string wrTime)
    {
        _matchInfo.WrHolderName = wrHolder;
        _matchInfo.WrTime = wrTime;
    }
    
    internal void IncreaseRoundCounter()
    {
        _matchInfo.NumRound++;
    }
    internal void ResetRoundCounter()
    {
        _matchInfo.NumRound = 0;
    }
    
    internal void IncreaseTrackCounter()
    {
        _matchInfo.NumTrack++;
    }
    
    internal void ResetTrackCounter()
    {
        _matchInfo.NumTrack = 0;
    }

    internal MatchInfo GetMatchInfo()
    {
        return _matchInfo;
    }


}
