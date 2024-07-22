using EvoSC.Common.Interfaces;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.TeamInfoModule.Interfaces;
using EvoSC.Modules.Official.TeamInfoModule.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.TeamInfoModule.Tests.Services;

public class TeamInfoServiceTests
{
    private readonly Mock<IManialinkManager> _manialinkManager = new();

    private readonly (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote)
        _server = Mocking.NewServerClientMock();

    private ITeamInfoService TeamInfoServiceMock()
    {
        return new TeamInfoService(_server.Client.Object, _manialinkManager.Object);
    }

    [Theory]
    [InlineData(7, 6, 0, 1, true)]
    [InlineData(7, 6, 5, 1, true)]
    [InlineData(7, 6, 6, 1, true)]
    [InlineData(7, 0, 0, 1, false)]
    [InlineData(7, 5, 0, 1, false)]
    [InlineData(7, 6, 5, 2, true)]
    [InlineData(7, 7, 6, 2, true)]
    [InlineData(7, 6, 6, 2, false)]
    [InlineData(7, 7, 7, 2, false)]
    public async Task DetectsMapPoint(int pointsLimit, int teamPoints, int opponentPoints, int pointsGap, bool shouldHaveMapPoint)
    {
        Assert.Equal(
            shouldHaveMapPoint,
            await TeamInfoServiceMock().DoesTeamHaveMatchPoint(teamPoints, opponentPoints, pointsLimit, pointsGap)
        );
    }
}
