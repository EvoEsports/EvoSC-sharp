using System;
using System.Runtime.CompilerServices;
using DefaultEcs;
using EvoSC.Core.Remote;
using EvoSC.Events;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.Threading;
using GameHost.V3.Utility;
using GbxRemoteNet.XmlRpc;
using NLog;
using EvoSC.ServerConnection;

namespace EvoSC.ServerConnection
{
    public class CreateEventServerSystem : AppSystem
    {
        private readonly IRunnableScheduler _scheduler = new ConcurrentScheduler();
        private IServerBeforeEventLoopSubscriber _beginLoop;
        private IServerAfterEventLoopSubscriber _endLoop;

        private EntitySet _eventSet;

        private PlayerSystem _playerSystem;
        private IGbxRemote _remote;

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
            Disposables.Add(_remote.SubscribeAnyEvent(OnRemoteEvent));

            Disposables.Add(_beginLoop.Subscribe(OnEventBegin).IntendedBox());
            Disposables.Add(_endLoop.Subscribe(OnEventEnd).IntendedBox());

            Disposables.Add(_eventSet = _world.GetEntities().With<EventTag>().AsSet());
        }

        private void OnRemoteEvent(in OnRemoteCallback message)
        {
            var args = message.Arguments;
            
            bool GetBool(int pos) => Unsafe.Unbox<bool>(XmlRpcTypes.ToNativeValue<bool>(args[pos]));
            string GetString(int pos) => (string)XmlRpcTypes.ToNativeValue<string>(args[pos]);

            bool Schedule<T>(Func<T, bool> ac, T args)
            {
                _scheduler.Add(ac, args, default);
                return true;
            }

            var matched = true;
            var scheduled = message.Method switch
            {
                "ManiaPlanet.PlayerChat" or "TrackMania.PlayerChat" =>
                    Schedule(args =>
                    {
                        var player = args.t._playerSystem.GetOrCreatePlayer(args.login);
                        args.t.CreateEventEntity().Set(new EventOnPlayerChat
                        {
                            Player = new PlayerEntity(player), Text = args.text
                        });

                        return true;
                    }, (t: this, /* ignore playerId */ login: GetString(1), text: GetString(2))),

                "ManiaPlanet.PlayerConnect" or "TrackMania.PlayerConnect" =>
                    Schedule(args =>
                    {
                        var player = args.t._playerSystem.GetOrCreatePlayer(args.login);
                        args.t.CreateEventEntity().Set(new EventOnPlayerConnect
                        {
                            Player = new PlayerEntity(player), IsSpectator = args.spectator
                        });

                        player.Set<InGameConnectedPlayer>();

                        return true;
                    }, (t: this, login: GetString(0), spectator: GetBool(1))),

                "ManiaPlanet.PlayerDisconnect" or "TrackMania.PlayerDisconnect" =>
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
                    }, (t: this, login: GetString(0), reason: GetString(1))),

                _ => matched = false
            };

            if (!matched)
            {
                _logger.Trace("Method not matched: {0}", message.Method);
            }
            else if (!scheduled)
            {
                _logger.Trace("Method not able to be scheduled: {0}", message.Method);
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
