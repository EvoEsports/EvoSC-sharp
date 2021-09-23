using System;
using DefaultEcs;
using GameHost.V3.Ecs;
using GameHost.V3.Loop.EventSubscriber;

namespace EvoSC.ServerConnection
{
    public interface IServerBeforeEventLoopSubscriber : IServerEventLoopSubscriber
    {
    }

    public interface IServerEventLoopSubscriber : IEventSubscriber
    {
        Entity Subscribe(Action action, ProcessOrder process = null);
    }

    public interface IServerAfterEventLoopSubscriber : IServerEventLoopSubscriber
    {
    }
}
