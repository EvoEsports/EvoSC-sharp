using DefaultEcs;
using EvoSC.Utility.Commands;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using EvoSC.ChatCommand;

namespace EvoSC.ChatCommand
{
    public class ChatCommandManager : AppSystem
    {
        private World _world;

        public ChatCommandManager(Scope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _world);
        }

        protected override void OnInit()
        {

        }

        public Entity Add(string command, ChatCommandInvoked callback, string description = "-",
            CommandArguments.Argument[] arguments = null,
            string access = null, bool hidden = false)
        {
            var entity = _world.CreateEntity();
            entity.Set<IsChatCommand>();
            entity.Set(new CommandPath(command));
            entity.Set(new CommandDescription(description));
            entity.Set(callback);

            if (hidden)
                entity.Set<ChatCommandIsHidden>();
            
            var list = new CommandArguments();
            if (arguments != null)
                list.AddRange(arguments);

            entity.Set(list);

            return entity;
        }
    }
}
