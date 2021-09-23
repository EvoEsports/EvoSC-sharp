using System;
using System.Threading.Tasks;
using DefaultEcs;
using EvoSC.Core.Remote;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.Threading;
using GameHost.V3.Utility;

namespace EvoSC.ServerConnection
{
    public class CreatePlayerOnServerStart : AppSystem
    {
        private World _world;
        private IGbxRemote _remote;

        // We ask for the domain-thread only task scheduler for GetPlayerList
        private ConstrainedTaskScheduler _taskScheduler;
        private ServerConnectionStateBindable _connectionState;

        private PlayerSystem _playerSystem;

        public CreatePlayerOnServerStart(Scope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _world);
            Dependencies.AddRef(() => ref _remote);
            Dependencies.AddRef(() => ref _taskScheduler);
            Dependencies.AddRef(() => ref _connectionState);
            Dependencies.AddRef(() => ref _playerSystem);
        }

        protected override void OnInit()
        {
            Disposables.AddRange(new IDisposable[]
            {
                _connectionState.Subscribe((_, state) =>
                {
                    if (state < ServerConnectionState.Connecting)
                        return;

                    // Each time we connect to the server we ask for the player list
                    _taskScheduler.StartUnwrap(async () =>
                    {
                        OnGetPlayerList(new[] {await _remote.GetMainServerPlayerInfoAsync()}, true);
                        OnGetPlayerList(await _remote.GetPlayerListAsync(), false);
                    });
                }, true)
            });
        }

        private void OnGetPlayerList(GbxPlayerInfo[] playersInfo, bool isServer)
        {
            foreach (var info in playersInfo)
            {
                var entity = _playerSystem.GetOrCreatePlayer(info.Login, false);
                PlayerEntity.SetPlayerInfo(entity, info);

                entity.Remove<Task<GbxPlayerInfo>>();
                
                if (isServer)
                    entity.Set<IsServerPlayer>();
            }
        }
    }
}
