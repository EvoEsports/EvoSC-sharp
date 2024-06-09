using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Modules.Official.MapQueueModule.Controllers;
using EvoSC.Modules.Official.MapQueueModule.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Moq;

namespace EvoSC.Modules.Official.MapQueueModuleTests.Tests.Controllers;

public class QueueCommandsControllerTests : CommandInteractionControllerTestBase<QueueCommandsController>
{
    private Mock<IOnlinePlayer> _player = new();
    private readonly Mock<IMapQueueService> _mapQueueServiceMock = new();
    private readonly Mock<IMapService> _mapServiceMock = new();
    private (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote) _server = Mocking.NewServerClientMock();
    
    public QueueCommandsControllerTests()
    {
        _player.Setup(m => m.AccountId).Returns("a467a996-eba5-44bf-9e2b-8543b50f39ae");
        InitMock(_player.Object, _mapQueueServiceMock, _mapServiceMock, _server.Client);
    }

    [Fact]
    public async Task Map_Is_Added_To_Queue()
    {
        var map = new Map { Uid = "map" };
        _mapServiceMock.Setup(m => m.GetMapByIdAsync(0)).Returns(Task.FromResult((IMap)map));

        await Controller.QueueAsync(0);
        
        _mapServiceMock.Verify(m => m.GetMapByIdAsync(0));
        _mapQueueServiceMock.Verify(m => m.EnqueueAsync(map));
    }
    
    [Fact]
    public async Task QueueList_Command_Gets_Queued_Maps_And_Sends_Chat_Message()
    {
        _mapQueueServiceMock.Setup(m => m.QueuedMaps).Returns([]);
        
        await Controller.QueueListAsync();
        
        _mapQueueServiceMock.VerifyGet(m => m.QueuedMaps, Times.Once);
    }
}
