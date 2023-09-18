using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Services;
using Moq;

namespace EvoSC.Modules.Official.OpenPlanetModule.Tests.Services;

public class OpenPlanetTrackerServiceTests
{
    [Fact]
    public void Player_Is_Added()
    {
        var service = new OpenPlanetTrackerService();
        var player = new Mock<IPlayer>();
        var opInfo = new Mock<IOpenPlanetInfo>();

        player.Setup(p => p.AccountId).Returns("something");
        player.Setup(p => p.Equals(It.IsAny<IPlayer>()))
            .Returns((IPlayer a) => player.Object.AccountId.Equals(a.AccountId));
        
        service.AddOrUpdatePlayer(player.Object, opInfo.Object);

        var addedPlayer = service.Players.FirstOrDefault();
        
        Assert.NotNull(addedPlayer);
        Assert.Equal(player.Object, addedPlayer.Player);
        Assert.Equal(opInfo.Object, addedPlayer.OpenPlanetInfo);
    }
    
    [Fact]
    public void Player_Is_Removed()
    {
        var service = new OpenPlanetTrackerService();
        var player = new Mock<IPlayer>();
        var opInfo = new Mock<IOpenPlanetInfo>();
        
        player.Setup(p => p.AccountId).Returns("something");
        player.Setup(p => p.Equals(It.IsAny<IPlayer>()))
            .Returns((IPlayer a) => player.Object.AccountId.Equals(a.AccountId));
        
        service.AddOrUpdatePlayer(player.Object, opInfo.Object);
        service.RemovePlayer(player.Object);

        var addedPlayer = service.Players.FirstOrDefault();
        
        Assert.Null(addedPlayer);
    }
    
    [Fact]
    public void Player_Is_Updated()
    {
        var service = new OpenPlanetTrackerService();
        var player = new Mock<IPlayer>();
        var opInfo1 = new Mock<IOpenPlanetInfo>();
        var opInfo2 = new Mock<IOpenPlanetInfo>();
        
        player.Setup(p => p.AccountId).Returns("something");
        player.Setup(p => p.Equals(It.IsAny<IPlayer>()))
            .Returns((IPlayer a) => player.Object.AccountId.Equals(a.AccountId));
        
        service.AddOrUpdatePlayer(player.Object, opInfo1.Object);
        service.AddOrUpdatePlayer(player.Object, opInfo2.Object);

        var addedPlayer = service.Players.FirstOrDefault();
        
        Assert.NotNull(addedPlayer);
        Assert.Equal(player.Object, addedPlayer.Player);
        Assert.Equal(opInfo2.Object, addedPlayer.OpenPlanetInfo);
    }
}
