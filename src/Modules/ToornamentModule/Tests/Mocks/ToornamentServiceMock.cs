using Bogus;
using EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Models;
using ToornamentApi.Models.Api.TournamentApi;

namespace ToornamentTest.Mocks
{
    public class ToornamentServiceMock : IToornamentService
    {
        List<DisciplineInfo> disciplines = new List<DisciplineInfo>();
        List<GroupInfo> groupInfos = new List<GroupInfo>();
        List<MatchInfo> matchInfos = new List<MatchInfo>();
        List<MatchGameInfo> matchGameInfos = new List<MatchGameInfo>();
        List<RoundInfo> roundInfos = new List<RoundInfo>();
        List<StageInfo> stageInfos = new List<StageInfo>();
        List<TournamentBasicData> tournamentInfos = new List<TournamentBasicData>();
        OpponentInfo[] opponentInfos = [];

        public ToornamentServiceMock(int nrOfTournaments = 1, int nrOfStages = 3, int nrOfGroups = 3, int nrOfRounds = 7, int nrOfMatches = 32, int nrOfMatchGames = 1, int nrOfDisciplines = 7, int nrOfOpponents = 300)
        {
            GenerateOpponents(nrOfOpponents);
            GenerateTournament(nrOfTournaments, nrOfStages, nrOfGroups, nrOfRounds, nrOfMatches, nrOfMatchGames, nrOfDisciplines);
        }

        private void GenerateTournament(int nrOfTournaments = 1, int nrOfStages = 3, int nrOfGroups = 3, int nrOfRounds = 7, int nrOfMatches = 32, int nrOfMatchGames = 1, int nrOfDisciplines = 7)
        {
            tournamentInfos = new Faker<TournamentBasicData>()
                    .RuleFor(t => t.Id, Guid.NewGuid().ToString())
                    .RuleFor(t => t.Discipline, f => f.Name.ToString())
                    .RuleFor(t => t.Name, f => f.Company.CompanyName())
                    .RuleFor(t => t.FullName, f => f.Company.CompanyName())
                    .RuleFor(t => t.Status, "pending")
                    .RuleFor(t => t.ScheduledDateStart, f => f.Date.Soon())
                    .RuleFor(t => t.ScheduledDateEnd, f => f.Date.Future())
                    .UseSeed(33888666)
                    .Generate(nrOfTournaments);

            foreach (var tournament in tournamentInfos)
            {
                GenerateStages(tournament.Id, nrOfStages, nrOfGroups, nrOfRounds, nrOfMatches);
            }
        }

        private void GenerateStages(string tournamentId, int nrOfStages, int nrOfGroups, int nrOfRounds, int nrOfMatches)
        {
            stageInfos = new Faker<StageInfo>()
                    .RuleFor(t => t.Id, Guid.NewGuid().ToString())
                    .RuleFor(t => t.TournamentId, tournamentId)
                    .RuleFor(t => t.Name, f => f.Company.CompanyName())
                    .RuleFor(t => t.Number, f => f.IndexFaker++)
                    .RuleFor(t => t.Type, "ffa")
                    .RuleFor(t => t.Closed, false)
                    .UseSeed(33888666)
                    .Generate(nrOfStages);

            foreach (var stage in stageInfos)
            {
                GenerateGroups(tournamentId, stage.Id, nrOfGroups, nrOfRounds, nrOfMatches);
            }
        }

        private void GenerateGroups(string tournamentId, string stageId, int nrOfGroups, int nrOfRounds, int nrOfMatches)
        {
            groupInfos = new Faker<GroupInfo>()
                    .RuleFor(t => t.Id, Guid.NewGuid().ToString())
                    .RuleFor(t => t.TournamentId, tournamentId)
                    .RuleFor(t => t.StageId, stageId)
                    .RuleFor(t => t.Name, f => f.Company.CompanyName())
                    .RuleFor(t => t.Number, f => f.IndexFaker++)
                    .RuleFor(t => t.Closed, false)
                    .UseSeed(33888666)
                    .Generate(nrOfGroups);

            foreach (var group in groupInfos)
            {
                GenerateRounds(tournamentId, stageId, group.Id, nrOfRounds, nrOfMatches);
            }
        }

        private void GenerateRounds(string tournamentId, string stageId, string groupId, int nrOfRounds, int nrOfMatches)
        {
            roundInfos = new Faker<RoundInfo>()
                    .RuleFor(t => t.Id, Guid.NewGuid().ToString())
                    .RuleFor(t => t.TournamentId, tournamentId)
                    .RuleFor(t => t.StageId, stageId)
                    .RuleFor(t => t.GroupId, groupId)
                    .RuleFor(t => t.Name, f => f.Company.CompanyName())
                    .RuleFor(t => t.Number, f => f.IndexFaker++)
                    .RuleFor(t => t.Closed, false)
                    .UseSeed(33888666)
                    .Generate(nrOfRounds);

            foreach (var round in roundInfos)
            {
                GenerateMatches(tournamentId, stageId, groupId, round.Id, nrOfMatches);
            }
        }

        private void GenerateMatches(string tournamentId, string stageId, string groupId, string roundId, int nrOfMatches)
        {
            matchInfos = new Faker<MatchInfo>()
                    .RuleFor(t => t.Id, Guid.NewGuid().ToString())
                    .RuleFor(t => t.TournamentId, tournamentId)
                    .RuleFor(t => t.StageId, stageId)
                    .RuleFor(t => t.GroupId, groupId)
                    .RuleFor(t => t.RoundId, roundId)
                    .RuleFor(t => t.Type, "ffa")
                    .RuleFor(t => t.Number, f => f.IndexFaker++)
                    .RuleFor(t => t.Status, "pending")
                    .RuleFor(t => t.Opponents, opponentInfos)
                    .UseSeed(33888666)
                    .Generate(nrOfMatches);

            foreach (var match in matchInfos)
            {
                GenerateMatchGames(match.Opponents);
            }
        }

        private void GenerateOpponents(int nrOfOpponents)
        {
            opponentInfos = new Faker<OpponentInfo>()
                    .RuleFor(t => t.Number, f => f.IndexFaker++)
                    .RuleFor(t => t.Position, f => f.IndexFaker++)
                    .RuleFor(t => t.Participant, (f, t) =>
                        {
                            return new Faker<ParticipantInfo>()
                                .RuleFor(t => t.Id, Guid.NewGuid().ToString())
                                .RuleFor(t => t.Name, f => f.Person.FullName)
                                .RuleFor(t => t.CustomUserIdentifier, f => f.Person.UserName)
                                .RuleFor(t => t.CustomFields, f =>
                                    {
                                        var dict = new Dictionary<string, object>();
                                        dict.Add("trackmania_id", Guid.NewGuid().ToString());
                                        return dict;
                                    })
                                .UseSeed(33888666)
                                .GenerateForever()
                                .Skip(f.IndexFaker++)
                                .Take(1)
                                .First();
                        }
                     )
                    .UseSeed(33888666)
                    .Generate(nrOfOpponents).ToArray();
        }

        private void GenerateMatchGames(OpponentInfo[] opponents)
        {
            matchGameInfos = new Faker<MatchGameInfo>()
                    .RuleFor(t => t.Number, 1)
                    .RuleFor(t => t.Opponents, opponents)
                    .UseSeed(33888666)
                    .Generate(matchInfos.Count);
        }

        public Task<DisciplineInfo?> GetDisciplineAsync(string disciplineId)
        {
            return Task.Run(() => disciplines.FirstOrDefault(x => x.Id == disciplineId));
        }

        public Task<GroupInfo?> GetGroupAsync(string groupId)
        {
            return Task.Run(() => groupInfos.FirstOrDefault(x => x.Id == groupId));
        }

        public Task<List<GroupInfo>?> GetGroupsAsync(string tournamentId, string stageId)
        {
            return Task.Run(() => groupInfos);
        }

        public Task<MatchInfo?> GetMatchAsync(string matchId)
        {
            return Task.Run(() => matchInfos.FirstOrDefault(x => x.Id == matchId));
        }

        public Task<List<MatchInfo>?> GetMatchesAsync(string tournamentId, string stageId)
        {
            return Task.Run(() => matchInfos);
        }

        public Task<MatchGameInfo?> GetMatchGameAsync(string matchId, int gameNumber)
        {
            return Task.Run(() => matchGameInfos.FirstOrDefault(x => x.Number == gameNumber));
        }

        public Task<List<MatchGameInfo>?> GetMatchGamesAsync(string matchId)
        {
            return Task.Run(() => matchGameInfos);
        }

        public Task<RoundInfo?> GetRoundAsync(string roundId)
        {
            return Task.Run(() => roundInfos.FirstOrDefault(x => x.Id == roundId));
        }

        public Task<List<RoundInfo>?> GetRoundsAsync(string tournamentId, string stageId)
        {
            return Task.Run(() => roundInfos);
        }

        public Task<StageInfo?> GetStageAsync(string stageId)
        {
            return Task.Run(() => stageInfos.FirstOrDefault(x => x.Id == stageId));
        }

        public Task<List<StageInfo>?> GetStagesAsync(string tournamentId)
        {
            return Task.Run(() => stageInfos);
        }

        public Task<TournamentBasicData?> GetTournamentAsync(string tournamentId)
        {
            return Task.Run(() => tournamentInfos.FirstOrDefault(x => x.Id == tournamentId));
        }

        public Task<List<TournamentBasicData>?> GetTournamentsAsync()
        {
            return Task.Run(() => tournamentInfos);
        }

        public Task<MatchGameInfo?> SetMatchGameResultAsync(string matchId, int gameNumber, MatchGameInfo gameInfo)
        {
            return Task.Run(() =>
            {
                var matchGameInfo = matchGameInfos.FirstOrDefault(x => x.Number == gameNumber);
                matchGameInfo = gameInfo;
                return matchGameInfo;
            });
        }

        public Task<MatchGameInfo?> SetMatchGameStatusAsync(string matchId, int gameNumber, MatchGameStatus status)
        {
            return Task.Run(() =>
            {
                var matchGameInfo = matchGameInfos.FirstOrDefault(x => x.Number == gameNumber);
                matchGameInfo.Status = status.ToString();
                return matchGameInfo;
            });
        }

        public Task<MatchGameInfo?> SetMatchGameMapAsync(string matchId, int gameNumber, string mapName)
        {
            return Task.Run(() =>
            {
                var matchGameInfo = matchGameInfos.FirstOrDefault(x => x.Number == gameNumber);

                if (matchGameInfo.Properties.ContainsKey("track"))
                {
                    matchGameInfo.Properties.Remove("track");
                }
                matchGameInfo.Properties.Add("track", mapName);
                return matchGameInfo;
            });
        }
    }
}
