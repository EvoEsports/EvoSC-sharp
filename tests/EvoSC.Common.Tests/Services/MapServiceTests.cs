using System.Threading.Tasks;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Repository.Maps;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EvoSC.Common.Tests.Services;

public class MapServiceTests
{
    private Mock<MapRepository> _mapRepository = new();
    private Mock<ILogger<MapService>> _logger = new();
    private Mock<IEvoScBaseConfig> _config = new();
    private Mock<IPlayerManagerService> _playerService = new();
    private (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote) _server = Mocking.NewServerClientMock();

    private MapService _mapService;

    public MapServiceTests()
    {
        _mapService = new MapService(_mapRepository.Object, _logger.Object, _config.Object, _playerService.Object,
            _server.Client.Object);
    }

    [Fact]
    public async Task Get_Map_By_Id()
    {
        var map = new DbMap
        {
            AuthorId = 1,
            Enabled = true,
            Id = 123,
            ExternalId = "1337",
            Name = "Snippens dream",
            Uid = "Uid"
        };
        _mapRepository.Setup(m => m.GetMapByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult((IMap)map)!);

        var retrievedMap = await _mapService.GetMapByIdAsync(123);
        
        Assert.Equal(retrievedMap.Id, map.Id);
    }
}
