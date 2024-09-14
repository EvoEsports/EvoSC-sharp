using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Middleware;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Permissions.Models;
using EvoSC.Common.Remote.ChatRouter;
using EvoSC.Modules.Official.TeamChatModule.Config;
using EvoSC.Modules.Official.TeamChatModule.Middlewares;
using Moq;

namespace TeamChatModule.Tests.Middlewares;

public class TeamChatMiddlewareTests
{
    (Mock<ITeamChatSettings> Settings, TeamChatMiddleware Middleware, ChatRouterPipelineContext Context, Mock<IOnlinePlayer> Actor) NewMock()
    {
        var action = new Mock<ActionDelegate>();
        var settings = new Mock<ITeamChatSettings>();
        var player = new Mock<IOnlinePlayer>();
        var context = new ChatRouterPipelineContext
        {
            ForwardMessage = false,
            Author = player.Object,
            MessageText = "",
            Recipients = [],
            IsTeamMessage = false
        };
        var middleware = new TeamChatMiddleware(action.Object, settings.Object);

        return (settings, middleware, context, player);
    }
    
    [Theory]
    [InlineData(PlayerTeam.Team1, 4)]
    [InlineData(PlayerTeam.Team2, 3)]
    public async Task Team_Chat_Enabled_Only_Includes_Team(PlayerTeam team, int count)
    {
        var mock = NewMock();

        mock.Actor.Setup(m => m.Team).Returns(team);
        mock.Context.IsTeamMessage = true;
        mock.Context.Recipients =
        [
            new OnlinePlayer { AccountId = "1", State = PlayerState.Playing, Team = PlayerTeam.Team1,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "2", State = PlayerState.Playing, Team = PlayerTeam.Team1,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "3", State = PlayerState.Playing, Team = PlayerTeam.Team2,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "4", State = PlayerState.Playing, Team = PlayerTeam.Team1,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "5", State = PlayerState.Playing, Team = PlayerTeam.Team2,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "6", State = PlayerState.Playing, Team = PlayerTeam.Team1,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "7", State = PlayerState.Playing, Team = PlayerTeam.Team2,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
        ];

        await mock.Middleware.ExecuteAsync(mock.Context);
        
        Assert.Equal(count, mock.Context.Recipients.Count);
    }

    [Theory]
    [InlineData(PlayerTeam.Team1, 5)]
    [InlineData(PlayerTeam.Team2, 3)]
    public async Task Players_In_Included_Group_Is_Always_Included(PlayerTeam team, int count)
    {
        var mock = NewMock();

        mock.Settings.Setup(m => m.IncludeGroup).Returns(2);
        mock.Actor.Setup(m => m.Team).Returns(team);
        mock.Context.IsTeamMessage = true;
        mock.Context.Recipients =
        [
            new OnlinePlayer { AccountId = "1", State = PlayerState.Playing, Team = PlayerTeam.Team1,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "2", State = PlayerState.Playing, Team = PlayerTeam.Team1,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "3", State = PlayerState.Playing, Team = PlayerTeam.Team2,Groups = new []{new Group{Id = 2, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "4", State = PlayerState.Playing, Team = PlayerTeam.Team1,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "5", State = PlayerState.Playing, Team = PlayerTeam.Team2,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "6", State = PlayerState.Playing, Team = PlayerTeam.Team1,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "7", State = PlayerState.Playing, Team = PlayerTeam.Team2,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
        ];
        
        await mock.Middleware.ExecuteAsync(mock.Context);
        
        Assert.Equal(count, mock.Context.Recipients.Count);
    }
    
    [Theory]
    [InlineData(PlayerTeam.Team1, 3)]
    [InlineData(PlayerTeam.Team2, 2)]
    public async Task Players_In_Excluded_Group_Is_Never_Included(PlayerTeam team, int count)
    {
        var mock = NewMock();

        mock.Settings.Setup(m => m.ExcludeGroup).Returns(2);
        mock.Actor.Setup(m => m.Team).Returns(team);
        mock.Context.IsTeamMessage = true;
        mock.Context.Recipients =
        [
            new OnlinePlayer { AccountId = "1", State = PlayerState.Playing, Team = PlayerTeam.Team1,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "2", State = PlayerState.Playing, Team = PlayerTeam.Team1,Groups = new []{new Group{Id = 2, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "3", State = PlayerState.Playing, Team = PlayerTeam.Team2,Groups = new []{new Group{Id = 2, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "4", State = PlayerState.Playing, Team = PlayerTeam.Team1,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "5", State = PlayerState.Playing, Team = PlayerTeam.Team2,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "6", State = PlayerState.Playing, Team = PlayerTeam.Team1,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
            new OnlinePlayer { AccountId = "7", State = PlayerState.Playing, Team = PlayerTeam.Team2,Groups = new []{new Group{Id = 1, Title = "", Description = ""}}},
        ];
        
        await mock.Middleware.ExecuteAsync(mock.Context);
        
        Assert.Equal(count, mock.Context.Recipients.Count);
    }
}
