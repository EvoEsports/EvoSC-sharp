using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.MapsModule.Controllers;
using EvoSC.Modules.Official.MapsModule.Events;
using EvoSC.Modules.Official.MapsModule.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace EvoSC.Modules.Official.MapsModule.Tests.Controllers;

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
        
        await Controller.AddMapAsync("123");
        
        _mxMapService.Verify(m => m.FindAndDownloadMapAsync(123, null, _actor.Object), Times.Once);
        AuditEventBuilder.Verify(m => m.Success(), Times.Once);
        AuditEventBuilder.Verify(m => m.WithEventName(AuditEvents.MapAdded), Times.Once);
        _server.Client.Verify(m => m.SuccessMessageAsync(_actor.Object, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Adding_Map_Failing_With_Exception_Is_Logged()
    {
        var ex = new Exception();
        _mxMapService.Setup(m => m.FindAndDownloadMapAsync(123, null, _actor.Object))
            .Throws(ex);

        await Assert.ThrowsAsync<Exception>(() => Controller.AddMapAsync("123"));
        
        _server.Client.Verify(m => m.ErrorMessageAsync(_actor.Object, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Adding_Duplicate_Map_Returns_Error()
    {
        var ex = new DuplicateMapException("Failed map add");
        _mxMapService.Setup(m => m.FindAndDownloadMapAsync(123, null, _actor.Object))
            .Throws(ex);

        await Assert.ThrowsAsync<DuplicateMapException>(() => Controller.AddMapAsync("123"));
        
        _server.Client.Verify(m => m.ErrorMessageAsync(_actor.Object, It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async Task Adding_Map_Failing_With_Null_Return_Is_Logged()
    {
        _mxMapService.Setup(m => m.FindAndDownloadMapAsync(123, null, _actor.Object))
            .Returns(Task.FromResult((IMap?)null));

        await Controller.AddMapAsync("123");
        
        _server.Client.Verify(m => m.ErrorMessageAsync(_actor.Object, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Map_Is_Removed()
    {
        _mapService.Setup(m => m.GetMapByIdAsync(123)).Returns(Task.FromResult(_map.Object));

        await Controller.RemoveMapAsync(123);
        
        _mapService.Verify(m => m.GetMapByIdAsync(123), Times.Once);
        _mapService.Verify(m => m.RemoveMapAsync(123), Times.Once);
        AuditEventBuilder.Verify(m => m.Success(), Times.Once);
        AuditEventBuilder.Verify(m => m.WithEventName(AuditEvents.MapRemoved), Times.Once);
        _server.Client.Verify(m => m.SuccessMessageAsync(_actor.Object, It.IsAny<string>()), Times.Once);
        _server.Client.Verify(m => m.ErrorMessageAsync(_actor.Object, It.IsAny<string>()), Times.Never);
        _logger.Verify(LogLevel.Debug, null, null, Times.Once());
    }

    [Fact]
    public async Task Map_Removal_Failed_Is_Reported()
    {
        _mapService.Setup(m => m.GetMapByIdAsync(123)).Returns(Task.FromResult((IMap?)null));

        await Controller.RemoveMapAsync(123);
        
        _mapService.Verify(m => m.RemoveMapAsync(123), Times.Never);
        _server.Client.Verify(m => m.SuccessMessageAsync(_actor.Object, It.IsAny<string>()), Times.Never);
        _server.Client.Verify(m => m.ErrorMessageAsync(_actor.Object, It.IsAny<string>()), Times.Once);
        AuditEventBuilder.Verify(m => m.Success(), Times.Never);
    }
}
