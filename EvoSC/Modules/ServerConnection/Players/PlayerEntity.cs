using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultEcs;
using EvoSC.Core.Remote;
using GameHost.V3.Utility;

namespace EvoSC.ServerConnection
{
    public struct PlayerEntity
    {
        public readonly Entity Backend;

        public ValueTask<InGamePlayerInfo> Info
        {
            get
            {
                if (Backend.TryGet(out InGamePlayerInfo component))
                {
                    return ValueTask.FromResult(component);
                }

                if (Backend.TryGet(out Task<GbxPlayerInfo> task))
                {
                    var backend = Backend;

                    async ValueTask<InGamePlayerInfo> GetTask()
                    {
                        var result = await task;
                        SetPlayerInfo(backend, result);
                        return backend.Get<InGamePlayerInfo>();
                    }

                    return GetTask();
                }

                throw new KeyNotFoundException($"Component {nameof(InGamePlayerInfo)} not found on {Backend}");
            }
        }

        public string Login
        {
            get
            {
                if (Backend.TryGet(out PlayerLogin component))
                    return component.Value;

                throw new KeyNotFoundException($"Component {nameof(PlayerLogin)} not found on {Backend}");
            }
        }

        public int Id
        {
            get
            {
                if (Backend.TryGet(out InGameConnectedPlayer component))
                    return component.Id;

                throw new KeyNotFoundException($"Component {nameof(InGameConnectedPlayer)} not found on {Backend}");
            }
        }

        public PlayerEntity(Entity backend)
        {
            Backend = backend;
        }

        public override string ToString()
        {
            return $"Player('{Login}', {Backend})";
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

        internal static void QueueInfoTask(Entity entity, string login, IGbxRemote client)
        {
            entity.Set(client.GetPlayerInfoAsync(login));
        }
    }
}
