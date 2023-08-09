using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.MatchManagerModule.Config;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchManagerModule.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MatchTracker : IMatchTracker
{
    private MatchStatus _status = MatchStatus.Unknown;
    private IMatchTimeline _currentTimelime;

    private readonly ITrackerSettings _settings;
    private readonly IPlayerManagerService _players;
    
    public MatchTracker(ITrackerSettings settings, IPlayerManagerService players)
    {
        _settings = settings;
        _players = players;
    }

    public IMatchTimeline LatestTimeline => _status != MatchStatus.Unknown && _currentTimelime != null
        ? _currentTimelime
        : throw new InvalidOperationException(
            "A match is currently not being tracked or is in an unknown state. Cannot return the timeline.");

    public async Task TrackScoresAsync(ScoresEventArgs scoreArgs)
    {
        if (!_settings.AutomaticTracking && _status is MatchStatus.Ended or MatchStatus.Unknown)
        {
            throw new InvalidOperationException("Trying to track a un-started match with automatic tracking disabled.");
        }

        if (_settings.AutomaticTracking && _status is MatchStatus.Ended or MatchStatus.Unknown)
        {
            await BeginMatchAsync();
        }

        IMatchState state = null;
        
        switch (scoreArgs.Section)
        {
            case ModeScriptSection.EndMap:
            case ModeScriptSection.EndMatch:
            case ModeScriptSection.EndRound:
            case ModeScriptSection.EndMatchEarly:
            case ModeScriptSection.PreEndRound:
                
                {
                    var playerScores = new List<IPlayerScore>();

                    foreach (var playerScore in scoreArgs.Players)
                    {
                        var player = await _players.GetPlayerAsync(playerScore.AccountId);
                        playerScores.Add(new PlayerScore
                        {
                            Player = player,
                            Rank = playerScore.Rank,
                            RoundPoints = playerScore.RoundPoints,
                            MapPoints = playerScore.MapPoints,
                            MatchPoints = playerScore.MatchPoints,
                            PreviousRaceTime = RaceTime.FromMilliseconds(playerScore.PreviousRaceTime),
                            BestRaceTime = RaceTime.FromMilliseconds(playerScore.BestRaceTime),
                            BestLapTime = RaceTime.FromMilliseconds(playerScore.BestLapTime),
                            BestRaceCheckpoints = playerScore.BestRaceCheckpoints.Select(RaceTime.FromMilliseconds),
                            PreviousRaceCheckpoints = playerScore.PreviousRaceCheckpoints.Select(RaceTime.FromMilliseconds),
                            BestLapCheckpoints = playerScore.BestLapCheckpoints.Select(RaceTime.FromMilliseconds)
                        });
                    }

                    state = new ScoresMatchState
                    {
                        Status = MatchStatus.Running,
                        Timestamp = DateTime.UtcNow,
                        Section = scoreArgs.Section,
                        Teams = scoreArgs.Teams.Select(t => new TeamScore
                        {
                            TeamId = t.Id,
                            TeamName = t.Name,
                            RoundPoints = t.RoundPoints,
                            MapPoints = t.MapPoints,
                            MatchPoints = t.MatchPoints
                        }),
                        Players = playerScores
                    };
                }
                break;
            default:
                state = new MatchState {Status = MatchStatus.Unknown, Timestamp = DateTime.UtcNow};
                break;
        }
        
        _currentTimelime.States.Add(state);

        if (scoreArgs.Section == ModeScriptSection.EndMatch && _settings.AutomaticMatchEnd)
        {
            await EndMatchAsync();
        }
    }

    public Task TrackChatMessageAsync()
    {
        throw new NotImplementedException();
    }

    public Task BeginMatchAsync()
    {
        _status = MatchStatus.Started;
        _currentTimelime = new MatchTimeline();
        return Task.CompletedTask;
    }

    public Task<IMatchTimeline> EndMatchAsync()
    {
        if (_status == MatchStatus.Ended)
        {
            throw new InvalidOperationException("Cannot end a match that has already ended.");
        }
        
        _status = MatchStatus.Ended;
        return Task.FromResult(_currentTimelime);
    }
}
