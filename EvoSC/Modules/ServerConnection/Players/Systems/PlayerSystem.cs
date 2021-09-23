using System;
using System.Threading.Tasks;
using DefaultEcs;
using EvoSC.Core.Remote;
using GameHost.V3;
using GameHost.V3.Domains.Time;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.Loop.EventSubscriber;
using GameHost.V3.Utility;
using NLog;

namespace EvoSC.ServerConnection
{
    public class PlayerSystem : AppSystem
    {
        private IGbxRemote _remote;
        private World _world;
        private IDomainUpdateLoopSubscriber _updateLoop;

        private ILogger _logger;

        public PlayerSystem(Scope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _remote);
            Dependencies.AddRef(() => ref _world);
            Dependencies.AddRef(() => ref _updateLoop);
            Dependencies.AddRef(() => ref _logger);

            scope.Context.Register(this);
        }

        private EntitySet _completedSet;
        private EntityMap<PlayerLogin> _waitingForInfoMap;
        private EntityMap<PlayerLogin> _playerMap;

        protected override void OnInit()
        {
            Disposables.AddRange(new IDisposable[]
            {
                _playerMap = _world.GetEntities()
                    .AsMap<PlayerLogin>(),
                _waitingForInfoMap = _world.GetEntities()
                    .With<Task<GbxPlayerInfo>>()
                    .AsMap<PlayerLogin>(),
                _completedSet = _world.GetEntities()
                    .With<Task<GbxPlayerInfo>>()
                    .With<TaskEntityFinishedTag>()
                    .AsSet(),
                _updateLoop.Subscribe(OnUpdate)
            });
        }

        private void OnUpdate(WorldTime time)
        {
            foreach (var playerLogin in _waitingForInfoMap.Keys)
            {
                var entity = _waitingForInfoMap[playerLogin];
                var task = entity.Get<Task<GbxPlayerInfo>>();
                switch (task.Status)
                {
                    case TaskStatus.RanToCompletion:
                        var result = task.Result;
                        entity.Set(new InGameConnectedPlayer(result.PlayerId));
                        entity.Set(new TaskEntityFinishedTag());

                        PlayerEntity.SetPlayerInfo(entity, task.Result);

                        break;
                    case TaskStatus.Canceled:
                    case TaskStatus.Faulted:

                        if (task.Status == TaskStatus.Faulted)
                        {
                            _logger.Error(task.Exception);
                        }

                        entity.Set(new TaskEntityFinishedTag());
                        break;
                }
            }

            _completedSet.Remove<Task<GbxPlayerInfo>, TaskEntityFinishedTag>();
        }

        public Entity GetOrCreatePlayer(string login, bool runInfoTask = true)
        {
            if (!_playerMap.TryGetEntity(new PlayerLogin(login), out var entity))
            {
                entity = _world.CreateEntity();
                entity.Set(new PlayerLogin(login));
            }

            if (runInfoTask && !entity.Has<InGamePlayerInfo>() && !entity.Has<Task<GbxPlayerInfo>>())
            {
                PlayerEntity.QueueInfoTask(entity, login, _remote);
            }

            return entity;
        }

        private struct TaskEntityFinishedTag
        {
        }
    }
}
