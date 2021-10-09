using System;
using System.Text;
using DefaultEcs;
using EvoSC.Modules.ChatCommand.Components;
using EvoSC.Utility.Commands;
using EvoSC.Utility.Remotes;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;

namespace EvoSC.Modules.ChatCommand.Systems
{
    public class ChatShowHelp : AppSystem
    {
        private ILowLevelGbxRemote _remote;
        private ChatCommandManager _manager;
        private World _world;

        public ChatShowHelp(Scope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _remote);
            Dependencies.AddRef(() => ref _manager);
            Dependencies.AddRef(() => ref _world);
        }

        protected override void OnInit()
        {
            _manager.Add("/help", (player, arguments) =>
            {
                ShowHelpAll(player.Login);
            }, "Global Help");
        }

        public void ShowHelpAll(string login)
        {
            using var commandSet = _world.GetEntities()
                .With<IsChatCommand>()
                .AsSet();

            ShowHelp(login, commandSet.GetEntities());
        }

        public void ShowHelp(string login, ReadOnlySpan<Entity> commands)
        {
            var sb = new StringBuilder();
            sb.AppendLine("$f17Available Commands:");
            foreach (var entity in commands)
            {
                if (entity.Has<ChatCommandIsHidden>())
                    continue;
                
                var path = entity.Get<CommandPath>().Value;
                var arguments = entity.Get<CommandArguments>();

                sb.Append("$fff$i");
                sb.Append(path);
                sb.Append(' ');
                foreach (var arg in arguments)
                {
                    sb.Append("$f17$i");
                    sb.Append(GetHumanType(arg.Type));
                    sb.Append("$ffe:$i$999");
                    sb.Append(arg.Name);
                    sb.Append(' ');
                }

                sb.AppendLine("$z$s");
            }

            _remote.ChatSendServerMessageToLoginAsync(sb.ToString(), login);
        }

        private string GetHumanType(Type type)
        {
            if (type == typeof(double))
                return "decimal";
            if (type == typeof(long))
                return "number";
            if (type == typeof(string))
                return "text";
            if (type == typeof(bool))
                return "yes/no";

            return type.Name.ToLower();
        }
    }
}