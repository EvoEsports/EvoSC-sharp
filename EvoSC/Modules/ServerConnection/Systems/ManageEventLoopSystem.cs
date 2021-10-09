using System;
using DefaultEcs;
using GameHost.V3;
using GameHost.V3.Domains.Time;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.Loop;
using GameHost.V3.Loop.EventSubscriber;
using GameHost.V3.Utility;

namespace EvoSC.Modules.ServerConnection
{
    public class ManageEventLoopSystem : AppSystem
    {
        private readonly Scope _scope;
        private EventLoop[] _eventLoops;
        private IDomainUpdateLoopSubscriber _updateLoop;
        private World _world;

        public ManageEventLoopSystem(Scope scope) : base(scope)
        {
            _scope = scope;

            Dependencies.AddRef(() => ref _world);
            Dependencies.AddRef(() => ref _updateLoop);
        }

        protected override void OnInit()
        {
            Disposables.AddRange(
                (_eventLoops = new EventLoop[] {new(_world), new(_world), new(_world)}) as IDisposable[]);
            Disposables.Add(_updateLoop.Subscribe(OnUpdate));

            _scope.Context.Register<IServerBeforeEventLoopSubscriber>(_eventLoops[0]);
            _scope.Context.Register<IServerEventLoopSubscriber>(_eventLoops[1]);
            _scope.Context.Register<IServerAfterEventLoopSubscriber>(_eventLoops[2]);
        }

        private void OnUpdate(WorldTime worldTime)
        {
            foreach (var loop in _eventLoops)
            {
                loop.Invoke();
            }
        }

        private class EventLoop : SimpleLoopSubscriber<Action>,
            IServerBeforeEventLoopSubscriber,
            IServerAfterEventLoopSubscriber
        {
            public EventLoop(World world) : base(world)
            {
            }

            public Entity Subscribe(Action<Entity> callback, ProcessOrder process = null)
            {
                return Subscribe(() => { callback(NonGenericEntity); }, process);
            }

            public Entity Subscribe(Action action, ProcessOrder process = null)
            {
                var entity = OrderGroup.Add(process);
                entity.Set(action);

                return entity;
            }

            protected override void OnInvoked(Span<Action> delegates)
            {
                foreach (var del in delegates)
                {
                    del();
                }
            }
        }
    }
}