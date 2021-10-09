using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultEcs;
using EvoSC.Utility.Remotes;
using EvoSC.Utility.Remotes.Structs;
using GameHost.V3.Utility;
using NLog;

namespace EvoSC.Modules.ServerConnection
{
    public struct PlayerEntity
    {
        public readonly Entity Entity;

        public bool HasInfo => Entity.Has<InGamePlayerInfo>() || Entity.Has<Task<GbxPlayerInfo>>();

        public readonly ValueTask<InGamePlayerInfo> GetInGameInfoAsync()
        {
            if (Entity.TryGet(out InGamePlayerInfo component))
            {
                return ValueTask.FromResult(component);
            }

            if (Entity.TryGet(out Task<GbxPlayerInfo> task))
            {
                var backend = Entity;

                async ValueTask<InGamePlayerInfo> GetTask()
                {
                    var result = await task;
                    SetPlayerInfo(backend, result);
                    return backend.Get<InGamePlayerInfo>();
                }

                return GetTask();
            }

            throw new KeyNotFoundException($"Component {nameof(InGamePlayerInfo)} not found on {Entity}");
        }

        public readonly InGamePlayerInfo GetInGameInfo()
        {
            var task = GetInGameInfoAsync();
            // fast path, result is already here and there will be no allocation
            if (task.IsCompleted)
                return task.Result;

            // slow path, wait for the task to be completed
            // (convert it to a managed task since ValueTask.Result may not block)
            return task.AsTask().Result;
        }

        public readonly string NickName
        {
            get
            {
                if (Entity.TryGet(out CustomPlayerNickName custom))
                    return custom;

                return GetInGameInfo().NickName;
            }

            set
            {
                if (value == null)
                    Entity.Remove<CustomPlayerNickName>();

                Entity.Set(new CustomPlayerNickName(value));
            }
        }

        public readonly string Login
        {
            get
            {
                if (Entity.TryGet(out PlayerLogin component))
                    return component.Value;

                throw new KeyNotFoundException($"Component {nameof(PlayerLogin)} not found on {Entity}");
            }
        }

        public readonly int Id
        {
            get
            {
                if (Entity.TryGet(out InGameConnectedPlayer component))
                    return component.Id;

                throw new KeyNotFoundException($"Component {nameof(InGameConnectedPlayer)} not found on {Entity}");
            }
        }

        public PlayerEntity(Entity entity)
        {
            Entity = entity;
        }

        public override string ToString()
        {
            return $"Player('{Login}', {Entity})";
        }

        internal static void SetPlayerInfo(Entity entity, GbxPlayerInfo result)
        {
            if (!entity.TryGet(out InGameConnectedPlayer serverPlayer) || serverPlayer.Id != result.PlayerId)
            {
                entity.Remove<InGameConnectedPlayer>();
                entity.Set(new InGameConnectedPlayer(result.PlayerId));
            }

            if (entity.TryGet(out PlayerLogin playerLogin))
            {
                if (playerLogin.Value != result.Login)
                    throw new InvalidOperationException(
                        $"Player Entities are not allowed to change login. (Ent={entity}, From {playerLogin.Value} To {result.PlayerId})"
                    );
            }
            else
            {
                entity.Remove<PlayerLogin>();
            }

            entity.Set(new InGamePlayerInfo
            (
                result.NickName,
                result.TeamId,
                result.LadderRanking,
                result.SpectatorStatus,
                result.Flags
            ));
        }

        internal static void QueueInfoTask(Entity entity, string login, ILowLevelGbxRemote client)
        {
            entity.Set(Task.Run(async () =>
            {
                var nullableInfo = await client.GetPlayerInfoAsync(login);
                if (nullableInfo is not { } info)
                {
                    // This may happen if the player disconnected
                    // or if they were never connected
                    
                    s_logger.Warn("Couldn't get the PlayerInfo of {login}", login);
                    return default;
                }

                return info;
            }));
        }

        private static readonly ILogger s_logger = LogManager.GetCurrentClassLogger();
    }
}
