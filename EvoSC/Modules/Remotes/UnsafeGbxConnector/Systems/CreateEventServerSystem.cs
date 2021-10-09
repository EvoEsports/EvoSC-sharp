using System;
using DefaultEcs;
using EvoSC.Events;
using EvoSC.Modules.Remotes.UnsafeGbxConnector.Impl;
using EvoSC.Modules.ServerConnection;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.Threading;
using GameHost.V3.Utility;
using NLog;
using UnsafeGbxConnector.Serialization;

namespace EvoSC.Modules.Remotes.UnsafeGbxConnector.Systems
{
    public class CreateEventServerSystem : AppSystem
    {
        private readonly IRunnableScheduler _scheduler = new ConcurrentScheduler();
        private IServerBeforeEventLoopSubscriber _beginLoop;
        private IServerAfterEventLoopSubscriber _endLoop;

        private EntitySet _eventSet;

        private PlayerSystem _playerSystem;
        private UnsafeLowLevelGbxRemote _remote;

        private World _world;

        private ILogger _logger;

        public CreateEventServerSystem(Scope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _world);
            Dependencies.AddRef(() => ref _remote);
            Dependencies.AddRef(() => ref _beginLoop);
            Dependencies.AddRef(() => ref _endLoop);
            Dependencies.AddRef(() => ref _playerSystem);
            Dependencies.AddRef(() => ref _logger);

            Disposables.Add(_scheduler);
        }

        protected override void OnInit()
        {
            _remote.Client.OnCallback += OnRemoteEvent;
            
            Disposables.Add(_beginLoop.Subscribe(OnEventBegin).IntendedBox());
            Disposables.Add(_endLoop.Subscribe(OnEventEnd).IntendedBox());

            Disposables.Add(_eventSet = _world.GetEntities().With<EventTag>().AsSet());
        }

        private void OnRemoteEvent(GbxCallback callback)
        {
            void Schedule<T>(Func<T, bool> ac, T args)
            {
                _scheduler.Add(ac, args, default);
            }

            var reader = callback.Reader;
            if (callback.Match("ManiaPlanet.PlayerChat", "TrackMania.PlayerChat"))
            {
                // ignore playerId (index 0)
                var login = reader[1].ReadString();
                var text = reader[2].ReadString();

                Schedule(args =>
                {
                    var player = args.t._playerSystem.GetOrCreatePlayer(args.playerLogin);
                    args.t.CreateEventEntity().Set(new EventOnPlayerChat
                    {
                        Player = new PlayerEntity(player), Text = args.text
                    });

                    return true;
                }, (t: this, playerLogin: login, text));
            }
            else if (callback.Match("ManiaPlanet.PlayerConnect", "TrackMania.PlayerConnect"))
            {
                var login = reader[0].ReadString();
                var spectator = reader[1].ReadBool();

                Schedule(args =>
                {
                    var player = args.t._playerSystem.GetOrCreatePlayer(args.login);
                    args.t.CreateEventEntity().Set(new EventOnPlayerConnect
                    {
                        Player = new PlayerEntity(player), IsSpectator = args.spectator
                    });

                    player.Set<InGameConnectedPlayer>();

                    return true;
                }, (t: this, login, spectator));
            }
            else if (callback.Match("ManiaPlanet.PlayerDisconnect", "TrackMania.PlayerDisconnect"))
            {
                var login = reader[0].ReadString();
                var reason = reader[1].ReadString();

                Schedule(args =>
                {
                    var player = args.t._playerSystem.GetOrCreatePlayer(args.login);
                    args.t.CreateEventEntity().Set(new EventOnPlayerDisconnect
                    {
                        Player = new PlayerEntity(player), Reason = args.reason
                    });

                    // Should we remove the component on the next update loop?
                    player.Remove<InGameConnectedPlayer>();

                    return true;
                }, (t: this, login, reason));
            }
            else if (_logger.IsTraceEnabled)
            {
                _logger.Trace("Callback not matched: {0}", callback.ToString());
            }
        }

        private Entity CreateEventEntity()
        {
            var entity = _world.CreateEntity();
            entity.Set<EventTag>();

            return entity;
        }

        private void OnEventBegin()
        {
            _scheduler.Run();
        }

        private void OnEventEnd()
        {
            _eventSet.DisposeAllEntities();
        }

        private struct EventTag
        {
        }
    }
}