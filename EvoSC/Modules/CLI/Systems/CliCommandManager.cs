using DefaultEcs;
using EvoSC.Utility.Commands;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using EvoSC.CLI;

namespace EvoSC.CLI
{
    public class CliCommandManager : AppSystem
    {
        private World _world;

        public CliCommandManager(Scope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _world);
        }

        protected override void OnInit()
        {

        }

        public Entity Add(string command, CliCommandInvoke callback, string description,
            CommandArguments.Argument[] arguments = null)
        {
            var ent = _world.CreateEntity();
            ent.Set(new IsCliCommand());
            ent.Set(new CommandPath(command));
            ent.Set(new CommandDescription(description));
            ent.Set(callback);

            var list = new CommandArguments();
            if (arguments != null)
                list.AddRange(arguments);

            ent.Set(list);
            return ent;
        }
    }
}
