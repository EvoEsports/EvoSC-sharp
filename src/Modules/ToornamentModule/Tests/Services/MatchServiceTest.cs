using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util.Auditing;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util.MatchSettings.Builders;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Models;
using EvoSC.Modules.EvoEsports.ToornamentModule.Services;
using EvoSC.Modules.EvoEsports.ToornamentModule.Settings;
using EvoSC.Modules.Official.MapsModule.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using EvoSC.Testing;
using GbxRemoteNet;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ToornamentApi.Models.Api.TournamentApi;
using ToornamentTest.Mocks;
using Xunit.Abstractions;

namespace Toornament.Services;

public class MatchServiceTest
{
    private readonly ITestOutputHelper _output;
    private readonly ILogger<MatchService> _loggerMock;
    private readonly Mock<IAuditService> _auditServiceMock = new();
    private readonly Mock<IServerClient> _serverClientMock = new();
    private readonly Mock<IManialinkManager> _manialinkManagerMock = new();
    private readonly Mock<IToornamentSettings> _toornamentSettingsMock = new();
    private readonly Mock<IMxMapService> _mxMapServiceMock = new();
    private readonly Mock<IMapService> _mapServiceMock = new();
    private readonly Mock<IPlayerManagerService> _playerManagerServiceMock = new();
    private readonly Mock<IPlayerReadyService> _playerReadyServiceMock = new();
    private readonly Mock<IPlayerReadyTrackerService> _playerReadyTrackerServiceMock = new();
    private readonly Mock<IStateService> _stateServiceMock = new();
    private readonly Mock<IMatchSettingsService> _matchSettingsMock = new();
    private readonly Mock<IMatchTracker> _matchTrackerMock = new();
    private readonly Mock<INadeoMapService> _nadeoMapServiceMock = new();
    private readonly IToornamentService _toornamentServiceMock;
    private readonly MatchService _matchService;
    private readonly Mock<IAuditEventBuilder> _auditEventBuilderMock;
    private readonly Mock<IKeyValueStoreService> _keyValueStoreServiceMock = new();
    private readonly Mock<IPermissionManager> _permissionManager = new();

    public MatchServiceTest(ITestOutputHelper output)
    {
        _output = output;

        _loggerMock = new NullLogger<MatchService>();
        //_toornamentServiceMock = new ToornamentServiceMock(1, 3, 3, 7, 32, 1, 7, 300);
        _toornamentServiceMock = new ToornamentServiceMock(1, 2, 2, 2, 2, 1, 1, 30);

        _matchService = new MatchService(_auditServiceMock.Object,
                                        _serverClientMock.Object,
                                        _manialinkManagerMock.Object,
                                        _toornamentSettingsMock.Object,
                                        _mxMapServiceMock.Object,
                                        _mapServiceMock.Object,
                                        _playerManagerServiceMock.Object,
                                        _playerReadyServiceMock.Object,
                                        _playerReadyTrackerServiceMock.Object,
                                        _stateServiceMock.Object,
                                        _matchSettingsMock.Object,
                                        _matchTrackerMock.Object,
                                        _toornamentServiceMock,
                                        _nadeoMapServiceMock.Object,
                                        _keyValueStoreServiceMock.Object,
                                        _permissionManager.Object,
                                        _loggerMock);

        _auditEventBuilderMock = Mocking.NewAuditEventBuilderMock();
    }

    [Fact]
    public async Task StartMatchAsync_Should_Start_Match()
    {
        //Arrange
        string matchId = "1";
        int gameNumber = 1;
        var matchGame = new MatchGameInfo()
        {
            Number = gameNumber,
            Opponents = [],
            Status = "running"
        };
        _matchTrackerMock.Setup(ts => ts.BeginMatchAsync()).ReturnsAsync(Guid.NewGuid());
        _toornamentSettingsMock.Setup(ts => ts.AssignedMatchId).Returns(matchId);
        _serverClientMock.Setup(sc => sc.Remote).Returns(new Mock<IGbxRemoteClient>().Object);
        _auditServiceMock.Setup(aus => aus.NewInfoEvent("Toornament.StartMatch")).Returns(_auditEventBuilderMock.Object);

        //Act
        await _matchService.StartMatchAsync();

        //Assert
        _matchTrackerMock.Verify(mt => mt.BeginMatchAsync(), Times.Once);
        _matchSettingsMock.Verify(ms => ms.LoadMatchSettingsAsync(_stateServiceMock.Object.MatchSettingsName, false), Times.Once);
        _serverClientMock.Verify(sc => sc.Remote.RestartMapAsync(), Times.Once);
        _playerReadyServiceMock.Verify(prs => prs.SetWidgetEnabled(false), Times.Once);
        _serverClientMock.Verify(sc => sc.Chat.InfoMessageAsync("Match is about to begin ..."), Times.Once);
        _stateServiceMock.Verify(s => s.SetMatchStarted(), Times.Once);

        var matchGameResponse = await _toornamentServiceMock.GetMatchGameAsync(matchId, gameNumber);
        Assert.Equal(matchGameResponse.Status, MatchGameStatus.Running.ToString());

        _auditServiceMock.Verify(aus => aus.NewInfoEvent("Toornament.StartMatch"), Times.Once);
    }

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 10)]
    public async Task EndMatchAsync_Should_End_Match(bool allPlayersShowedUp, int nrOfPlayersWhoShowedUp)
    {
        //Arrange

        //Setup Toornament Data
        string toornamentId = (await _toornamentServiceMock.GetTournamentsAsync()).First().Id;
        string stageId = (await _toornamentServiceMock.GetStagesAsync(toornamentId)).First().Id;
        string matchId = (await _toornamentServiceMock.GetMatchesAsync(toornamentId, stageId)).First().Id;
        int gameNumber = 1;
        var matchGameResponse = await _toornamentServiceMock.GetMatchGameAsync(matchId, gameNumber);

        //Setup Trackmania match results
        var playerScores = new List<PlayerScore>();
        int rank = 1;
        int score = 100 * matchGameResponse.Opponents.Length;
        int playerCounter = 0;
        foreach (var opponent in matchGameResponse.Opponents)
        {
            if (!allPlayersShowedUp && playerCounter == nrOfPlayersWhoShowedUp)
            {
                break;
            }

            var playerScore = new PlayerScore()
            {
                AccountId = opponent.Participant.CustomFields["trackmania_id"].ToString(),
                Name = opponent.Participant.Name,
                Rank = rank,
                MatchPoints = score,
            };
            playerScores.Add(playerScore);

            rank++;
            score -= 100;
            playerCounter++;
        }

        var timeline = new ScoresEventArgs()
        {
            Players = playerScores,
            Section = EvoSC.Common.Models.ModeScriptSection.EndMatch,
            Teams = new List<TeamScore>(),
            UseTeams = false,
            WinnerPlayer = playerScores.First().Name,
            WinnerTeam = 0,
        };

        //Setup mocks
        _toornamentSettingsMock.Setup(ts => ts.AssignedMatchId).Returns(matchId);

        //Act
        await _matchService.EndMatchAsync(timeline);

        //Assert
        var result = await _toornamentServiceMock.GetMatchGameAsync(matchId, gameNumber);
        foreach (var opponent in result.Opponents)
        {
            var player = playerScores.FirstOrDefault(p => p.AccountId == opponent.Participant.CustomFields["trackmania_id"].ToString());
            if (player == null)
            {
                Assert.Equal(opponent.Score, 1);
                _output.WriteLine($"Opponent {opponent.Participant.Name} did NOT play and got rank: {opponent.Rank}, with a score of: {opponent.Score}");
            }
            else
            {
                Assert.Equal(opponent.Rank, player.Rank);
                _output.WriteLine($"Opponent {opponent.Participant.Name} rank  was: {opponent.Rank}, Player {player.Name} rank  was: {player.Rank}");
                Assert.Equal(opponent.Score, player.MatchPoints);
                _output.WriteLine($"Opponent {opponent.Participant.Name} score was: {opponent.Score}, Player {player.Name} score was: {player.MatchPoints}");
            }
        }
        _serverClientMock.Verify(sc => sc.Chat.SuccessMessageAsync("Match finished, thanks for playing!"), Times.Once);
    }

    [Fact]
    public async Task ShowSetupScreenAsync_With_No_ToornamentId_Should_Show_The_Setup_Screen()
    {
        //Arrange
        var tournamentId = string.Empty;
        var stageId = string.Empty;

        var expectedTournaments = await _toornamentServiceMock.GetTournamentsAsync();
        List<StageInfo> expectedStages = [];
        List<MatchInfo> expectedMatches = [];
        List<GroupInfo> expectedGroups = [];
        List<RoundInfo> expectedRounds = [];

        var player = new Mock<IPlayer>();

        //Act
        await _matchService.ShowSetupScreenAsync(player.Object, tournamentId, stageId);

        //Assert
        _manialinkManagerMock.Verify(mm => mm.SendManialinkAsync(It.IsAny<IPlayer>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        _output.WriteLine($"Expected TournamentCount:  {expectedTournaments.Count}; StageCount:  {expectedStages.Count}; MatchCount:  {expectedMatches.Count}; GroupCount:  {expectedGroups.Count}; RoundCount:  {expectedRounds.Count};");

        var invocationObject = _manialinkManagerMock.Invocations[0].Arguments[2];

        var actualTournaments = invocationObject.GetType().GetProperty("tournaments").GetValue(invocationObject, null) as List<TournamentBasicData>;
        var actualStages = invocationObject.GetType().GetProperty("stages").GetValue(invocationObject, null) as List<StageInfo>;
        var actualMatches = invocationObject.GetType().GetProperty("matches").GetValue(invocationObject, null) as List<MatchInfo>;
        var actualGroups = invocationObject.GetType().GetProperty("groups").GetValue(invocationObject, null) as List<GroupInfo>;
        var actualRounds = invocationObject.GetType().GetProperty("rounds").GetValue(invocationObject, null) as List<RoundInfo>;

        _output.WriteLine($"Actual TournamentCount:  {actualTournaments.Count}; StageCount:  {actualStages.Count}; MatchCount:  {actualMatches.Count}; GroupCount:  {actualGroups.Count}; RoundCount:  {actualRounds.Count};");
    }

    [Fact]
    public async Task ShowSetupScreenAsync_With_ToornamentId_Should_Show_The_Setup_Screen()
    {
        //Arrange
        var tournamentId = string.Empty;
        var stageId = string.Empty;

        var expectedTournaments = await _toornamentServiceMock.GetTournamentsAsync();
        List<StageInfo> expectedStages = [];
        List<MatchInfo> expectedMatches = [];
        List<GroupInfo> expectedGroups = [];
        List<RoundInfo> expectedRounds = [];

        tournamentId = (await _toornamentServiceMock.GetTournamentsAsync()).FirstOrDefault().Id;
        expectedStages = await _toornamentServiceMock.GetStagesAsync(tournamentId);

        var player = new Mock<IPlayer>();

        //Act
        await _matchService.ShowSetupScreenAsync(player.Object, tournamentId, stageId);

        //Assert
        _manialinkManagerMock.Verify(mm => mm.SendManialinkAsync(It.IsAny<IPlayer>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        _output.WriteLine($"Expected TournamentCount:  {expectedTournaments.Count}; StageCount:  {expectedStages.Count}; MatchCount:  {expectedMatches.Count}; GroupCount:  {expectedGroups.Count}; RoundCount:  {expectedRounds.Count};");

        var invocationObject = _manialinkManagerMock.Invocations[0].Arguments[2];

        var actualTournaments = invocationObject.GetType().GetProperty("tournaments").GetValue(invocationObject, null) as List<TournamentBasicData>;
        var actualStages = invocationObject.GetType().GetProperty("stages").GetValue(invocationObject, null) as List<StageInfo>;
        var actualMatches = invocationObject.GetType().GetProperty("matches").GetValue(invocationObject, null) as List<MatchInfo>;
        var actualGroups = invocationObject.GetType().GetProperty("groups").GetValue(invocationObject, null) as List<GroupInfo>;
        var actualRounds = invocationObject.GetType().GetProperty("rounds").GetValue(invocationObject, null) as List<RoundInfo>;

        _output.WriteLine($"Actual TournamentCount:  {actualTournaments.Count}; StageCount:  {actualStages.Count}; MatchCount:  {actualMatches.Count}; GroupCount:  {actualGroups.Count}; RoundCount:  {actualRounds.Count};");
    }

    [Fact]
    public async Task ShowSetupScreenAsync_With_ToornamentId_And_StageId_Should_Show_The_Setup_Screen()
    {
        //Arrange
        var tournamentId = string.Empty;
        var stageId = string.Empty;

        var expectedTournaments = await _toornamentServiceMock.GetTournamentsAsync();
        List<StageInfo> expectedStages = [];
        List<MatchInfo> expectedMatches = [];
        List<GroupInfo> expectedGroups = [];
        List<RoundInfo> expectedRounds = [];

        tournamentId = (await _toornamentServiceMock.GetTournamentsAsync()).FirstOrDefault().Id;
        expectedStages = await _toornamentServiceMock.GetStagesAsync(tournamentId);

        stageId = (await _toornamentServiceMock.GetStagesAsync(tournamentId)).FirstOrDefault().Id;

        expectedMatches = await _toornamentServiceMock.GetMatchesAsync(tournamentId, stageId);
        expectedGroups = await _toornamentServiceMock.GetGroupsAsync(tournamentId, stageId);
        expectedRounds = await _toornamentServiceMock.GetRoundsAsync(tournamentId, stageId);

        var player = new Mock<IPlayer>();

        //Act
        await _matchService.ShowSetupScreenAsync(player.Object, tournamentId, stageId);

        //Assert
        _manialinkManagerMock.Verify(mm => mm.SendManialinkAsync(It.IsAny<IPlayer>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        _output.WriteLine($"Expected TournamentCount:  {expectedTournaments.Count}; StageCount:  {expectedStages.Count}; MatchCount:  {expectedMatches.Count}; GroupCount:  {expectedGroups.Count}; RoundCount:  {expectedRounds.Count};");

        var invocationObject = _manialinkManagerMock.Invocations[0].Arguments[2];

        var actualTournaments = invocationObject.GetType().GetProperty("tournaments").GetValue(invocationObject, null) as List<TournamentBasicData>;
        var actualStages = invocationObject.GetType().GetProperty("stages").GetValue(invocationObject, null) as List<StageInfo>;
        var actualMatches = invocationObject.GetType().GetProperty("matches").GetValue(invocationObject, null) as List<MatchInfo>;
        var actualGroups = invocationObject.GetType().GetProperty("groups").GetValue(invocationObject, null) as List<GroupInfo>;
        var actualRounds = invocationObject.GetType().GetProperty("rounds").GetValue(invocationObject, null) as List<RoundInfo>;

        _output.WriteLine($"Actual TournamentCount:  {actualTournaments.Count}; StageCount:  {actualStages.Count}; MatchCount:  {actualMatches.Count}; GroupCount:  {actualGroups.Count}; RoundCount:  {actualRounds.Count};");
    }


    [Fact]
    public async Task ShowSetupScreenAsync_With_ToornamentId_From_Settings_Should_Show_The_Setup_Screen()
    {
        //Arrange
        var tournamentId = string.Empty;
        var stageId = string.Empty;

        var expectedTournaments = await _toornamentServiceMock.GetTournamentsAsync();
        List<StageInfo> expectedStages = [];
        List<MatchInfo> expectedMatches = [];
        List<GroupInfo> expectedGroups = [];
        List<RoundInfo> expectedRounds = [];

        _toornamentSettingsMock.Setup(s => s.ToornamentId).Returns((await _toornamentServiceMock.GetTournamentsAsync()).FirstOrDefault().Id);
        tournamentId = _toornamentSettingsMock.Object.ToornamentId;

        expectedStages = await _toornamentServiceMock.GetStagesAsync(tournamentId);
        stageId = (await _toornamentServiceMock.GetStagesAsync(tournamentId)).FirstOrDefault().Id;

        expectedMatches = await _toornamentServiceMock.GetMatchesAsync(tournamentId, stageId);
        expectedGroups = await _toornamentServiceMock.GetGroupsAsync(tournamentId, stageId);
        expectedRounds = await _toornamentServiceMock.GetRoundsAsync(tournamentId, stageId);

        var player = new Mock<IPlayer>();

        //Act
        await _matchService.ShowSetupScreenAsync(player.Object, tournamentId, stageId);

        //Assert
        _manialinkManagerMock.Verify(mm => mm.SendManialinkAsync(It.IsAny<IPlayer>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        _output.WriteLine($"Expected TournamentCount:  {expectedTournaments.Count}; StageCount:  {expectedStages.Count}; MatchCount:  {expectedMatches.Count}; GroupCount:  {expectedGroups.Count}; RoundCount:  {expectedRounds.Count};");

        var invocationObject = _manialinkManagerMock.Invocations[0].Arguments[2];

        var actualTournaments = invocationObject.GetType().GetProperty("tournaments").GetValue(invocationObject, null) as List<TournamentBasicData>;
        var actualStages = invocationObject.GetType().GetProperty("stages").GetValue(invocationObject, null) as List<StageInfo>;
        var actualMatches = invocationObject.GetType().GetProperty("matches").GetValue(invocationObject, null) as List<MatchInfo>;
        var actualGroups = invocationObject.GetType().GetProperty("groups").GetValue(invocationObject, null) as List<GroupInfo>;
        var actualRounds = invocationObject.GetType().GetProperty("rounds").GetValue(invocationObject, null) as List<RoundInfo>;

        _output.WriteLine($"Actual TournamentCount:  {actualTournaments.Count}; StageCount:  {actualStages.Count}; MatchCount:  {actualMatches.Count}; GroupCount:  {actualGroups.Count}; RoundCount:  {actualRounds.Count};");
    }

    [Fact]
    public async Task SetupServerAsync_With_Invalid_ToornamentId_Should_Throw_ArgumentNullException()
    {
        //Arrange
        var player = new Mock<IPlayer>();

        //Act

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _matchService.SetupServerAsync(player.Object, string.Empty, string.Empty, string.Empty));
    }

    [Fact]
    public async Task SetupServerAsync_With_Invalid_StageId_Should_Throw_ArgumentNullException()
    {
        //Arrange
        var player = new Mock<IPlayer>();
        var tournamentId = "";

        tournamentId = (await _toornamentServiceMock.GetTournamentsAsync()).FirstOrDefault().Id;
        //Act

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _matchService.SetupServerAsync(player.Object, tournamentId, string.Empty, string.Empty));
    }

    [Fact]
    public async Task SetupServerAsync_With_Invalid_MatchId_Should_Throw_ArgumentNullException()
    {
        //Arrange
        var player = new Mock<IPlayer>();
        var tournamentId = string.Empty;
        var stageId = string.Empty;

        tournamentId = (await _toornamentServiceMock.GetTournamentsAsync()).FirstOrDefault().Id;
        stageId = (await _toornamentServiceMock.GetStagesAsync(tournamentId)).FirstOrDefault().Id;
        //Act

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _matchService.SetupServerAsync(player.Object, tournamentId, stageId, string.Empty));
    }

    [Fact]
    public async Task SetupServerAsync_With_No_MapUids_Should_Throw_ArgumentException()
    {
        //Arrange
        var player = new Mock<IPlayer>();
        var mapIds = string.Empty;

        var tournamentId = (await _toornamentServiceMock.GetTournamentsAsync()).FirstOrDefault().Id;
        var stageId = (await _toornamentServiceMock.GetStagesAsync(tournamentId)).FirstOrDefault().Id;
        var matchId = (await _toornamentServiceMock.GetMatchesAsync(tournamentId, stageId)).FirstOrDefault().Id;

        _mapServiceMock.Setup(x => x.GetMapByUidAsync(It.IsAny<string>())).ReturnsAsync(default(IMap));
        _toornamentSettingsMock.Setup(x => x.MapUids).Returns(mapIds);
        //Act
        await Assert.ThrowsAsync<ArgumentException>(() => _matchService.SetupServerAsync(player.Object, tournamentId, stageId, matchId));

        //Assert
        _mapServiceMock.Verify(mm => mm.GetMapByUidAsync(It.IsAny<string>()), Times.Never);
        _nadeoMapServiceMock.Verify(nm => nm.FindAndDownloadMapAsync(It.IsAny<string>()), Times.Never);
        _mxMapServiceMock.Verify(nm => nm.FindAndDownloadMapAsync(It.IsAny<int>(), null, player.Object), Times.Never);
    }

    [Fact]
    public async Task SetupServerAsync_With_Empty_MatchSettings_Should_Throw_ArgumentException()
    {
        //Arrange
        var player = new Mock<IPlayer>();
        var mapUids = "zS5d30EU7x6meq2eIM5Uhu4UbTg,VbW1c9rTPSwtZNHrlDE8FVPqIca,lKJQ8YrXza3XiEN1a1fPK3fl4wf,W6GjI5Nsr9MYdBBOBXIOd8JhZwj,YUTy7o9O0hDmWFNVQ4QuxaXzXD4";
        var mapUidsCount = mapUids.Split(',').Length;
        var mapMock = new Mock<IMap>();
        var disciplines = string.Empty;

        var tournamentId = (await _toornamentServiceMock.GetTournamentsAsync()).FirstOrDefault().Id;
        var stageId = (await _toornamentServiceMock.GetStagesAsync(tournamentId)).FirstOrDefault().Id;
        var matchId = (await _toornamentServiceMock.GetMatchesAsync(tournamentId, stageId)).FirstOrDefault().Id;

        _mapServiceMock.Setup(x => x.GetMapByUidAsync(It.IsAny<string>())).ReturnsAsync(mapMock.Object);
        _toornamentSettingsMock.Setup(x => x.MapUids).Returns(mapUids);
        _toornamentSettingsMock.Setup(x => x.UseToornamentDiscipline).Returns(false);
        _toornamentSettingsMock.Setup(x => x.Disciplines).Returns(disciplines);
        //Act
        await Assert.ThrowsAsync<ArgumentNullException>(() => _matchService.SetupServerAsync(player.Object, tournamentId, stageId, matchId));

        //Assert
        _mapServiceMock.Verify(mm => mm.GetMapByUidAsync(It.IsAny<string>()), Times.Exactly(mapUidsCount));
        _nadeoMapServiceMock.Verify(nm => nm.FindAndDownloadMapAsync(It.IsAny<string>()), Times.Never);
        _mxMapServiceMock.Verify(nm => nm.FindAndDownloadMapAsync(It.IsAny<int>(), null, player.Object), Times.Never);
    }


    [Fact]
    public async Task SetupServerAsync_With_UnknownMatchDisciplines_Should_Throw_ArgumentException()
    {
        //Arrange
        var player = new Mock<IPlayer>();
        var mapUids = "zS5d30EU7x6meq2eIM5Uhu4UbTg,VbW1c9rTPSwtZNHrlDE8FVPqIca,lKJQ8YrXza3XiEN1a1fPK3fl4wf,W6GjI5Nsr9MYdBBOBXIOd8JhZwj,YUTy7o9O0hDmWFNVQ4QuxaXzXD4";
        var mapUidsCount = mapUids.Split(',').Length;
        var mapMock = new Mock<IMap>();
        var disciplines = "[{\"game_mode\": \"rounds\",\"group_number\": 1,\"plugins\":{\"S_UseAutoReady\": false}, \"round_number\": 1,\"scripts\": {\"S_DelayBeforeNextMap\": 2000, \"S_FinishTimeout\": 3, \"S_MapsPerMatch\": 2, \"S_NbOfWinners\": 1, \"S_PointsLimit\": -1, \"S_PointsRepartition\": \"n-1\", \"S_RespawnBehaviour\": 0, \"S_RoundsPerMap\": 1, \"S_UseTieBreak\": false, \"S_WarmUpDuration\": 5, \"S_WarmUpNb\": 1}, \"stage_number\": 1, \"tracks_shuffle\": true}]";

        var tournamentId = (await _toornamentServiceMock.GetTournamentsAsync()).FirstOrDefault().Id;
        var stageId = (await _toornamentServiceMock.GetStagesAsync(tournamentId)).FirstOrDefault().Id;
        var matchId = (await _toornamentServiceMock.GetMatchesAsync(tournamentId, stageId)).FirstOrDefault().Id;

        _mapServiceMock.Setup(x => x.GetMapByUidAsync(It.IsAny<string>())).ReturnsAsync(mapMock.Object);
        _toornamentSettingsMock.Setup(x => x.MapUids).Returns(mapUids);
        _toornamentSettingsMock.Setup(x => x.UseToornamentDiscipline).Returns(false);
        _toornamentSettingsMock.Setup(x => x.Disciplines).Returns(disciplines);
        //Act
        await Assert.ThrowsAsync<ArgumentNullException>(() => _matchService.SetupServerAsync(player.Object, tournamentId, stageId, matchId));

        //Assert
        _mapServiceMock.Verify(mm => mm.GetMapByUidAsync(It.IsAny<string>()), Times.Exactly(mapUidsCount));
        _nadeoMapServiceMock.Verify(nm => nm.FindAndDownloadMapAsync(It.IsAny<string>()), Times.Never);
        _mxMapServiceMock.Verify(nm => nm.FindAndDownloadMapAsync(It.IsAny<int>(), null, player.Object), Times.Never);
    }

    [Fact]
    public async Task SetupServerAsync_Should_Setup_Server()
    {
        //Arrange
        var player = new Mock<IPlayer>();
        var mapUids = "zS5d30EU7x6meq2eIM5Uhu4UbTg,VbW1c9rTPSwtZNHrlDE8FVPqIca,lKJQ8YrXza3XiEN1a1fPK3fl4wf,W6GjI5Nsr9MYdBBOBXIOd8JhZwj,YUTy7o9O0hDmWFNVQ4QuxaXzXD4";
        var mapUidsCount = mapUids.Split(',').Length;
        var mapMock = new Mock<IMap>();

        var tournamentId = (await _toornamentServiceMock.GetTournamentsAsync()).FirstOrDefault().Id;
        var stage = (await _toornamentServiceMock.GetStagesAsync(tournamentId)).FirstOrDefault();
        var stageId = stage.Id;
        var stageNumber = stage.Number;
        var match = (await _toornamentServiceMock.GetMatchesAsync(tournamentId, stageId)).FirstOrDefault();
        var matchId = match.Id;
        var matchNumber = match.Number;
        var groupNumber = (await _toornamentServiceMock.GetGroupsAsync(tournamentId, stageId)).FirstOrDefault().Number;
        var roundNumber = (await _toornamentServiceMock.GetRoundsAsync(tournamentId, stageId)).FirstOrDefault().Number;

        var disciplines = "[{\"game_mode\": \"rounds\",\"group_number\": " + groupNumber + ",\"plugins\":{\"S_UseAutoReady\": false}, \"round_number\": " + roundNumber + ",\"scripts\": {\"S_DelayBeforeNextMap\": 2000, \"S_FinishTimeout\": 3, \"S_MapsPerMatch\": 2, \"S_NbOfWinners\": 1, \"S_PointsLimit\": -1, \"S_PointsRepartition\": \"n-1\", \"S_RespawnBehaviour\": 0, \"S_RoundsPerMap\": 1, \"S_UseTieBreak\": false, \"S_WarmUpDuration\": 5, \"S_WarmUpNb\": 1}, \"stage_number\": " + stageNumber + ", \"tracks_shuffle\": true}]";

        _mapServiceMock.Setup(x => x.GetMapByUidAsync(It.IsAny<string>())).ReturnsAsync(mapMock.Object);
        _toornamentSettingsMock.Setup(x => x.MapUids).Returns(mapUids);
        _toornamentSettingsMock.Setup(x => x.UseToornamentDiscipline).Returns(false);
        _toornamentSettingsMock.Setup(x => x.Disciplines).Returns(disciplines);
        _serverClientMock.Setup(sc => sc.Remote).Returns(new Mock<IGbxRemoteClient>().Object);
        _playerManagerServiceMock.Setup(pm => pm.GetOrCreatePlayerAsync(It.IsAny<string>())).ReturnsAsync((string input) => new Player() { AccountId = input });
        _playerManagerServiceMock.Setup(pm => pm.GetOnlinePlayersAsync()).ReturnsAsync(new List<IOnlinePlayer>() { new OnlinePlayer() { State = EvoSC.Common.Interfaces.Models.Enums.PlayerState.Playing } });

        //Act
        await _matchService.SetupServerAsync(player.Object, tournamentId, stageId, matchId);

        //Assert
        _mapServiceMock.Verify(mm => mm.GetMapByUidAsync(It.IsAny<string>()), Times.Exactly(mapUidsCount));
        _nadeoMapServiceMock.Verify(nm => nm.FindAndDownloadMapAsync(It.IsAny<string>()), Times.Never);
        _mxMapServiceMock.Verify(nm => nm.FindAndDownloadMapAsync(It.IsAny<int>(), null, player.Object), Times.Never);
        _matchSettingsMock.Verify(ms => ms.CreateMatchSettingsAsync(It.IsAny<string>(), It.IsAny<Action<MatchSettingsBuilder>>()), Times.Once);
        _serverClientMock.Verify(sc => sc.Remote.CleanGuestListAsync(), Times.Once);
        _serverClientMock.Verify(sc => sc.Remote.MultiCallAsync(It.IsAny<MultiCall>()));
        _matchSettingsMock.Verify(sc => sc.LoadMatchSettingsAsync(It.IsAny<string>()), Times.Once);
        _serverClientMock.Verify(sc => sc.Remote.SetServerNameAsync("Match #" + stageNumber + "." + groupNumber + "." + roundNumber + "." + matchNumber), Times.Once);
        _playerReadyServiceMock.Verify(sc => sc.ResetReadyWidgetAsync(It.IsAny<bool>()), Times.Once);
        _playerReadyTrackerServiceMock.Verify(sc => sc.AddRequiredPlayersAsync(It.IsAny<List<IPlayer>>()), Times.Once);
        _playerReadyServiceMock.Verify(sc => sc.SetWidgetEnabled(It.IsAny<bool>()), Times.Once);
        _playerManagerServiceMock.Verify(sc => sc.GetOnlinePlayersAsync(), Times.Once);
        _playerReadyServiceMock.Verify(sc => sc.SendWidgetAsync(It.IsAny<IOnlinePlayer>()));
        _stateServiceMock.Verify(state => state.SetInitialSetup(It.IsAny<string>()), Times.Once);
        _manialinkManagerMock.Verify(mm => mm.HideManialinkAsync(player.Object, It.IsAny<string>()), Times.Once);
    }
}
