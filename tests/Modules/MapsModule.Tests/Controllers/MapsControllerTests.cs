using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.MapsModule.Controllers;
using EvoSC.Modules.Official.MapsModule.Events;
using EvoSC.Modules.Official.MapsModule.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace EvoSC.Modules.Official.MapsModule.Tests.Controllers;

public class MapsControllerTests : CommandInteractionControllerTestBase<MapsController>
{
    private readonly IOnlinePlayer _actor = Substitute.For<IOnlinePlayer>();
    private readonly ILogger<MapsController> _logger = Substitute.For<ILogger<MapsController>>();
    private readonly IMxMapService _mxMapService = Substitute.For<IMxMapService>();
    private readonly IMapService _mapService = Substitute.For<IMapService>();
    private (IServerClient Client, IGbxRemoteClient Remote) _server = Mocking.NewServerClientMock();
    private readonly IMap? _map;

    public MapsControllerTests()
    {
        var locale = Mocking.NewLocaleMock(ContextService);
        InitMock(_actor, _logger, _mxMapService, _mapService, _server.Client, locale);
        
        _map = Substitute.For<IMap?>();
        _map?.Name.Returns("MyMap");
        _map?.Author.Returns(_actor);
    }
    
    [Fact]
    public async Task Map_Is_Added()
    {
        _mxMapService.FindAndDownloadMapAsync(123, null, _actor)
            .Returns(Task.FromResult(_map));
        
        await Controller.AddMapAsync("123");
        
        await _mxMapService.Received(1).FindAndDownloadMapAsync(123, null, _actor);
        AuditEventBuilder.Received(1).Success();
        AuditEventBuilder.Received(1).WithEventName(AuditEvents.MapAdded);
        await _server.Client.Received(1).SuccessMessageAsync(Arg.Any<string>(), _actor);
    }

    [Fact]
    public async Task Adding_Map_Failing_With_Exception_Is_Logged()
    {
        var ex = new Exception();
        _mxMapService.FindAndDownloadMapAsync(123, null, _actor)
            .ThrowsAsync(ex);

        await Assert.ThrowsAsync<Exception>(() => Controller.AddMapAsync("123"));
        
        await _server.Client.Received(1).ErrorMessageAsync(Arg.Any<string>(), _actor);
    }

    [Fact]
    public async Task Adding_Duplicate_Map_Returns_Error()
    {
        var ex = new DuplicateMapException("Failed map add");
        _mxMapService.FindAndDownloadMapAsync(123, null, _actor)
            .ThrowsAsync(ex);

        await Assert.ThrowsAsync<DuplicateMapException>(() => Controller.AddMapAsync("123"));
        
        await _server.Client.Received(1).ErrorMessageAsync(Arg.Any<string>(), _actor);
    }
    
    [Fact]
    public async Task Adding_Map_Failing_With_Null_Return_Is_Logged()
    {
        _mxMapService.FindAndDownloadMapAsync(123, null, _actor)
            .Returns(Task.FromResult((IMap?)null));

        await Controller.AddMapAsync("123");
        
        await _server.Client.Received(1).ErrorMessageAsync(Arg.Any<string>(), _actor);
    }

    [Fact]
    public async Task Map_Is_Removed()
    {
        _mapService.GetMapByIdAsync(123).Returns(Task.FromResult(_map));

        await Controller.RemoveMapAsync(123);
        
        await _mapService.Received(1).GetMapByIdAsync(123);
        await _mapService.Received(1).RemoveMapAsync(123);
        AuditEventBuilder.Received(1).Success();
        AuditEventBuilder.Received(1).WithEventName(AuditEvents.MapRemoved);
        await _server.Client.Received(1).SuccessMessageAsync(Arg.Any<string>(), _actor);
        await _server.Client.DidNotReceive().ErrorMessageAsync(Arg.Any<string>(), _actor);
        _logger.Verify(LogLevel.Debug, null, null, Quantity.Exactly(1));
    }

    [Fact]
    public async Task Map_Removal_Failed_Is_Reported()
    {
        _mapService.GetMapByIdAsync(123).Returns(Task.FromResult((IMap?)null));

        await Controller.RemoveMapAsync(123);
        
        await _mapService.DidNotReceive().RemoveMapAsync(123);
        await _server.Client.DidNotReceive().SuccessMessageAsync(Arg.Any<string>(), _actor);
        await _server.Client.Received(1).ErrorMessageAsync(Arg.Any<string>(), _actor);
        AuditEventBuilder.DidNotReceive().Success();
    }
}
