using System;
using System.Threading.Tasks;
using EvoSC.Common.Events;
using Microsoft.Extensions.Logging;
using Xunit;

namespace EvoSC.Common.Tests;

public class EventManagerTests
{
    class HandlerRanException : Exception {}
    class HandlerRanException2 : Exception {}
    
    [Fact]
    public async Task Event_Added_AndFired_Dont_Throw_Exception()
    {
        var manager = new EventManager(LoggerFactory.Create(c => {}).CreateLogger<EventManager>(), null);

        manager.Subscribe("test", (object sender, EventArgs args) =>
        {
            Console.WriteLine();
            return Task.CompletedTask;
        });
        var ex = await Record.ExceptionAsync(() => manager.Fire("test", EventArgs.Empty));
        
        Assert.Null(ex);
    }

    [Fact]
    public void Event_Added_And_Fired()
    {
        var manager = new EventManager(LoggerFactory.Create(c => {}).CreateLogger<EventManager>(), null);

        manager.Subscribe<EventArgs>("test", (sender, args) => throw new HandlerRanException());

        Assert.ThrowsAsync<HandlerRanException>(async () =>
        {
            await manager.Fire("test", EventArgs.Empty);
        });
    }

    [Fact]
    public async Task Event_Added_And_Removed()
    {
        var manager = new EventManager(LoggerFactory.Create(c => {}).CreateLogger<EventManager>(), null);

        var handler = new AsyncEventHandler<EventArgs>((sender, args) => throw new HandlerRanException());
        
        manager.Subscribe("test", handler);
        manager.Unsubscribe("test", handler);

        var ex = await Record.ExceptionAsync(() => manager.Fire("test", EventArgs.Empty));
        
        Assert.Null(ex);
    }

    [Fact]
    public void Only_Equal_Event_Handler_Removed()
    {
        var manager = new EventManager(LoggerFactory.Create(c => {}).CreateLogger<EventManager>(), null);

        var handler = new AsyncEventHandler<EventArgs>((sender, args) => throw new HandlerRanException());
        var handler2 = new AsyncEventHandler<EventArgs>((sender, args) => throw new HandlerRanException2());
        
        manager.Subscribe("test", handler);
        manager.Subscribe("test", handler2);
        manager.Unsubscribe("test", handler);

        Assert.ThrowsAsync<HandlerRanException2>(async () =>
        {
            await manager.Fire("test", EventArgs.Empty);
        });
    }
}
