using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection;
using GameHost.V3.Utility;
using EvoSC.Core.Plugins;
using EvoSC.Modules.ServerConnection;
using EvoSC.Utility.Remotes;

namespace Plugin.Samples.Simple
{
    //
    // PlayersConnection:
    //
    // Say a message with the nicknames of all connected players when created
    //
    public class ShowConnectedPlayers : PluginSystemBase
    {
        [Dependency] private World _world;
        [Dependency] private TaskScheduler _taskScheduler;

        public ShowConnectedPlayers(Scope scope) : base(scope)
        {
        }

        protected override void OnInit()
        {
            _taskScheduler.StartUnwrap(async () =>
            {
                using var set = _world.GetEntities()
                    .With<InGameConnectedPlayer>()
                    .Without<IsServerPlayer>()
                    .AsSet();

                while (set.GetEntities().IsEmpty)
                    await Task.Yield();
                
                var sb = new StringBuilder("Connected:");
                foreach (var entity in set.GetEntities().ToArray())
                {
                    var player = new PlayerEntity(entity);
                    sb.Append('\n');
                    sb.Append(player.NickName);
                    sb.Append(' ');
                    sb.Append('-');
                    sb.Append(' ');
                    sb.Append(player.Login);
                }

                Remote.SendChatMessage(sb.ToString());
            });
        }
    }
}