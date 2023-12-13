using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.MatchTrackerModule.Config;
using EvoSC.Modules.Official.MatchTrackerModule.Events;
using EvoSC.Modules.Official.MatchTrackerModule.Events.EventArgObjects;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Models;

namespace EvoSC.Modules.Official.MatchTrackerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MatchTracker(ITrackerSettings settings, IPlayerManagerService players, ITrackerStoreService trackerStore,
        IEventManager events)
    : IMatchTracker
{
    private MatchStatus _status = MatchStatus.Unknown;
    private IMatchTimeline _currentTimeline;

    public bool IsTracking { get; private set; }
    
    public IMatchTimeline LatestTimeline => _status != MatchStatus.Unknown && _currentTimeline != null
        ? _currentTimeline
        : throw new InvalidOperationException(
            "A match is currently not being tracked or is in an unknown state. Cannot return the timeline.");

    public MatchStatus Status => _status;

    public async Task TrackScoresAsync(ScoresEventArgs scoreArgs)
    {
        if (!IsTracking)
        {
            return;
        }
        
        await VerifyTracker();
        
        IMatchState state;

        switch (scoreArgs.Section)
        {
            case ModeScriptSection.EndMap when !settings.RecordEndMap:
            case ModeScriptSection.EndMatch when !settings.RecordEndMatch:
            case ModeScriptSection.EndRound when !settings.RecordEndRound:
            case ModeScriptSection.EndMatchEarly when !settings.RecordEndMatchEarly:
            case ModeScriptSection.PreEndRound when !settings.RecordPreEndRound:
                return;
            case ModeScriptSection.Undefined:
                state = new MatchState {Status = MatchStatus.Unknown, Timestamp = DateTime.UtcNow, TimelineId = _currentTimeline.TimelineId};
                break;
            default:
                {
                    var playerScores = new List<IPlayerScore>();

                    foreach (var playerScore in scoreArgs.Players)
                    {
                        var player = await players.GetPlayerAsync(playerScore.AccountId);
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
                            TeamId = t?.Id ?? 0,
                            TeamName = t?.Name ?? "",
                            RoundPoints = t?.RoundPoints ?? 0,
                            MapPoints = t?.MapPoints ?? 0,
                            MatchPoints = t?.MatchPoints ?? 0
                        }),
                        Players = playerScores,
                        TimelineId = _currentTimeline.TimelineId
                    };
                    break;
                }
        }
        
        _currentTimeline.States.Add(state);

        if (settings.ImmediateStoring)
        {
            await trackerStore.SaveState(state);
        }
        
        await events.RaiseAsync(MatchTrackerEvent.StateTracked,
            new MatchStateTrackedEventArgs {Timeline = _currentTimeline, State = state}, this);

        if (scoreArgs.Section == ModeScriptSection.EndMatch && settings.AutomaticMatchEnd)
        {
            await EndMatchAsync();
        }
    }

    public async Task<Guid> BeginMatchAsync()
    {
        if (IsTracking)
        {
            await EndMatchAsync();
        }
        
        _status = MatchStatus.Started;
        _currentTimeline = new MatchTimeline();
        IsTracking = true;

        var state = new MatchState
        {
            TimelineId = _currentTimeline.TimelineId, Status = MatchStatus.Started, Timestamp = DateTime.UtcNow
        };
        
        _currentTimeline.States.Add(state);
        
        await events.RaiseAsync(MatchTrackerEvent.StateTracked,
            new MatchStateTrackedEventArgs {Timeline = _currentTimeline, State = state}, this);

        if (settings.ImmediateStoring)
        {
            await trackerStore.SaveState(state);
        }

        return _currentTimeline.TimelineId;
    }

    public async Task<IMatchTimeline> EndMatchAsync()
    {
        if (!IsTracking)
        {
            throw new InvalidOperationException("Cannot end a match that has already ended.");
        }
        
        _status = MatchStatus.Ended;
        IsTracking = false;

        var state = new MatchState
        {
            TimelineId = _currentTimeline.TimelineId, Status = MatchStatus.Ended, Timestamp = DateTime.UtcNow
        };
        
        _currentTimeline.States.Add(state);
        
        await events.RaiseAsync(MatchTrackerEvent.StateTracked,
            new MatchStateTrackedEventArgs {Timeline = _currentTimeline, State = state}, this);

        if (settings.ImmediateStoring)
        {
            await trackerStore.SaveState(state);
        }
        else
        {
            await trackerStore.SaveTimelineAsync(_currentTimeline);
        }
        
        return _currentTimeline;
    }

    private async Task VerifyTracker()
    {
        if (!settings.AutomaticTracking && !IsTracking)
        {
            throw new InvalidOperationException("Trying to track a un-started match with automatic tracking disabled.");
        }

        if (settings.AutomaticTracking && !IsTracking)
        {
            await BeginMatchAsync();
        }
    }
}
