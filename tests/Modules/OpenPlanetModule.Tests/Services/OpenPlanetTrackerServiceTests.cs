using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Services;
using NSubstitute;

namespace EvoSC.Modules.Official.OpenPlanetModule.Tests.Services;

public class OpenPlanetTrackerServiceTests
{
    [Fact]
    public void Player_Is_Added()
    {
        var service = new OpenPlanetTrackerService();
        var player = Substitute.For<IPlayer>();
        var opInfo = Substitute.For<IOpenPlanetInfo>();

        player.AccountId.Returns("something");
        player.Equals(Arg.Any<IPlayer>())
            .ReturnsForAnyArgs(p => player.AccountId.Equals(p.Arg<IPlayer>().AccountId));
        
        service.AddOrUpdatePlayer(player, opInfo);

        var addedPlayer = service.Players.FirstOrDefault();
        
        Assert.NotNull(addedPlayer);
        Assert.Equal(player, addedPlayer.Player);
        Assert.Equal(opInfo, addedPlayer.OpenPlanetInfo);
    }
    
    [Fact]
    public void Player_Is_Removed()
    {
        var service = new OpenPlanetTrackerService();
        var player = Substitute.For<IPlayer>();
        var opInfo = Substitute.For<IOpenPlanetInfo>();
        
        player.AccountId.Returns("something");
        player.Equals(Arg.Any<IPlayer>())
            .ReturnsForAnyArgs(p => player.AccountId.Equals(p.Arg<IPlayer>().AccountId));
        
        service.AddOrUpdatePlayer(player, opInfo);
        service.RemovePlayer(player);

        var addedPlayer = service.Players.FirstOrDefault();
        
        Assert.Null(addedPlayer);
    }
    
    [Fact]
    public void Player_Is_Updated()
    {
        var service = new OpenPlanetTrackerService();
        var player = Substitute.For<IPlayer>();
        var opInfo1 = Substitute.For<IOpenPlanetInfo>();
        var opInfo2 = Substitute.For<IOpenPlanetInfo>();
        
        player.AccountId.Returns("something");
        player.Equals(Arg.Any<IPlayer>())
            .ReturnsForAnyArgs(p => player.AccountId.Equals(p.Arg<IPlayer>().AccountId));
        
        service.AddOrUpdatePlayer(player, opInfo1);
        service.AddOrUpdatePlayer(player, opInfo2);

        var addedPlayer = service.Players.FirstOrDefault();
        
        Assert.NotNull(addedPlayer);
        Assert.Equal(player, addedPlayer.Player);
        Assert.Equal(opInfo2, addedPlayer.OpenPlanetInfo);
    }
}
