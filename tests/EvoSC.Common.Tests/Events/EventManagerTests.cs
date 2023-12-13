using System;
using System.Threading.Tasks;
using EvoSC.Common.Events;
using EvoSC.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SimpleInjector;
using Xunit;

namespace EvoSC.Common.Tests.Events;

public class EventManagerTests
{
    class HandlerRanException : Exception {}
    class HandlerRanException2 : Exception {}

    private readonly ILogger<EventManager> _logger;
    private readonly IServiceProvider _services;
    private readonly Mock<IEvoSCApplication> _app;

    public EventManagerTests()
    {
        _logger = LoggerFactory.Create(c => { }).CreateLogger<EventManager>();
        _services = new ServiceCollection().BuildServiceProvider();
        _app = new Mock<IEvoSCApplication>();
        _app.Setup(a => a.Services).Returns(new Container());
    }
    
    [Fact]
    public async Task Event_Added_AndFired_Dont_Throw_Exception()
    {
        IEventManager manager = new EventManager(_logger, _app.Object, null);

        manager.Subscribe("test", (object sender, EventArgs args) =>
        {
            Console.WriteLine();
            return Task.CompletedTask;
        });
        var ex = await Record.ExceptionAsync(() => manager.RaiseAsync("test", EventArgs.Empty));
        
        Assert.Null(ex);
    }

    [Fact]
    public async Task Event_Added_And_Fired()
    {
        IEventManager manager = new EventManager(_logger, _app.Object, null);

        var result = 0;
        
        manager.Subscribe<EventArgs>("test", (sender, args) =>
        {
            result = 76;
            return Task.CompletedTask;
        });

        await manager.RaiseAsync("test", EventArgs.Empty);
        
        Assert.Equal(76, result);
    }

    [Fact]
    public async Task Event_Added_And_Removed()
    {
        IEventManager manager = new EventManager(_logger, _app.Object, null);

        var handler = new AsyncEventHandler<EventArgs>((sender, args) => throw new HandlerRanException());
        
        manager.Subscribe("test", handler);
        manager.Unsubscribe("test", handler);

        var ex = await Record.ExceptionAsync(() => manager.RaiseAsync("test", EventArgs.Empty));
        
        Assert.Null(ex);
    }

    [Fact]
    public async Task Only_Equal_Event_Handler_Removed()
    {
        IEventManager manager = new EventManager(_logger, _app.Object, null);

        var result = 0;

        var handler = new AsyncEventHandler<EventArgs>((sender, args) =>
        {
            result = 32;
            return Task.CompletedTask;
        });
        
        var handler2 = new AsyncEventHandler<EventArgs>((sender, args) =>
        {
            result = 64;
            return Task.CompletedTask;

        });
        
        manager.Subscribe("test", handler);
        manager.Subscribe("test", handler2);
        manager.Unsubscribe("test", handler);
        
        await manager.RaiseAsync("test", EventArgs.Empty);
        
        Assert.Equal(64, result);
    }
}
