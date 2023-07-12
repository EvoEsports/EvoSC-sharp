using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Modules.Official.Maps.Controllers;
using EvoSC.Modules.Official.Maps.Events;
using EvoSC.Modules.Official.Maps.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace EvoSC.Modules.Official.MapsModule.Tests;

public class MapsControllerTests : CommandInteractionControllerTestBase<MapsController>
{
    private Mock<IOnlinePlayer> _actor = new();
    private Mock<ILogger<MapsController>> _logger = new();
    private Mock<IMxMapService> _mxMapService = new();
    private Mock<IMapService> _mapService = new();
    private Locale _locale;
    private (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote) _server = Mocking.NewServerClientMock();
    private Mock<IMap?> _map;

    public MapsControllerTests()
    {
        _locale = Mocking.NewLocaleMock(ContextService.Object);
        InitMock(_actor.Object, _logger, _mxMapService, _mapService, _server.Client, _locale);
        
        _map = new Mock<IMap?>();
        _map.Setup(m => m.Name).Returns("MyMap");
        _map.Setup(m => m.Author).Returns(_actor.Object);
    }
    
    [Fact]
    public async Task Map_Is_Added()
    {
        _mxMapService.Setup(m => m.FindAndDownloadMapAsync(123, null, _actor.Object))
            .Returns(Task.FromResult(_map.Object));
        
        await Controller.AddMap("123");
        
        _mxMapService.Verify(m => m.FindAndDownloadMapAsync(123, null, _actor.Object), Times.Once);
        AuditEventBuilder.Verify(m => m.Success(), Times.Once);
        AuditEventBuilder.Verify(m => m.WithEventName(AuditEvents.MapAdded), Times.Once);
        _server.Client.Verify(m => m.SuccessMessageAsync(It.IsAny<string>(), _actor.Object), Times.Once);
    }

    [Fact]
    public async Task Adding_Map_Failing_With_Exception_Is_Logged()
    {
        var ex = new Exception();
        _mxMapService.Setup(m => m.FindAndDownloadMapAsync(123, null, _actor.Object))
            .Returns(() => throw ex);

        await Controller.AddMap("123");
        
        _logger.Verify(LogLevel.Information, ex, null, Times.Once());
        _server.Client.Verify(m => m.ErrorMessageAsync(It.IsAny<string>(), _actor.Object), Times.Once);
    }
    
    [Fact]
    public async Task Adding_Map_Failing_With_Null_Return_Is_Logged()
    {
        _mxMapService.Setup(m => m.FindAndDownloadMapAsync(123, null, _actor.Object))
            .Returns(Task.FromResult((IMap?)null));

        await Controller.AddMap("123");
        
        _server.Client.Verify(m => m.ErrorMessageAsync(It.IsAny<string>(), _actor.Object), Times.Once);
    }

    [Fact]
    public async Task Map_Is_Removed()
    {
        _mapService.Setup(m => m.GetMapByIdAsync(123)).Returns(Task.FromResult(_map.Object));

        await Controller.RemoveMap(123);
        
        _mapService.Verify(m => m.GetMapByIdAsync(123), Times.Once);
        _mapService.Verify(m => m.RemoveMapAsync(123), Times.Once);
        AuditEventBuilder.Verify(m => m.Success(), Times.Once);
        AuditEventBuilder.Verify(m => m.WithEventName(AuditEvents.MapRemoved), Times.Once);
        _server.Client.Verify(m => m.SuccessMessageAsync(It.IsAny<string>(), _actor.Object), Times.Once);
        _server.Client.Verify(m => m.ErrorMessageAsync(It.IsAny<string>(), _actor.Object), Times.Never);
        _logger.Verify(LogLevel.Information, null, null, Times.Once());
    }

    [Fact]
    public async Task Map_Removal_Failed_Is_Reported()
    {
        _mapService.Setup(m => m.GetMapByIdAsync(123)).Returns(Task.FromResult((IMap?)null));

        await Controller.RemoveMap(123);
        
        _mapService.Verify(m => m.RemoveMapAsync(123), Times.Never);
        _server.Client.Verify(m => m.SuccessMessageAsync(It.IsAny<string>(), _actor.Object), Times.Never);
        _server.Client.Verify(m => m.ErrorMessageAsync(It.IsAny<string>(), _actor.Object), Times.Once);
        AuditEventBuilder.Verify(m => m.Success(), Times.Never);
    }
}
