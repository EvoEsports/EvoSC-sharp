using DefaultEcs;
using EvoSC.Modules.ChatCommand.Components;
using EvoSC.Utility.Commands;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;

namespace EvoSC.Modules.ChatCommand.Systems
{
    /// <summary>
    /// Manager that can create chat commands
    /// </summary>
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

        /// <summary>
        /// Add a new command on the controller
        /// </summary>
        /// <param name="command">The command path</param>
        /// <param name="callback">Callback when the command has been triggered</param>
        /// <param name="description">The command description</param>
        /// <param name="arguments">The arguments of the command</param>
        /// <param name="access">The required accesses for the command</param>
        /// <param name="hidden">Whether or not the command is hidden from help pages</param>
        /// <returns>Command Entity</returns>
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