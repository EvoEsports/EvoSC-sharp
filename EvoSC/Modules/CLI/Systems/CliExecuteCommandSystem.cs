using System;
using System.Linq;
using Collections.Pooled;
using DefaultEcs;
using EvoSC.Utility.Commands;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using EvoSC.CLI;

namespace EvoSC.CLI
{
    public class CliExecuteCommandSystem : AppSystem
    {
        private World _world;

        public CliExecuteCommandSystem(Scope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _world);
        }

        protected override void OnInit()
        {
            // skip the first argument which is the .dll
            var arguments = Environment.GetCommandLineArgs()[1..];
            
            var input = string.Join(string.Empty, arguments.ToArray());

            using var set = _world.GetEntities()
                .With<IsCliCommand>()
                .AsSet();
            
            using var matches = new PooledList<Entity>();
            if (!CommandUtility.GetBestCommands(set.GetEntities(), input, matches))
            {
                ShowHelp();
                return;
            }

            var commandExecuted = false;
            var quit = true;
            
            using var argList = new PooledList<Entity>();
            foreach (var command in matches)
            {
                if (CommandUtility.CanExecuteCommand(command, input, argList))
                {
                    if (command.Get<CliCommandInvoke>()(argList.Span))
                        quit = false;

                    commandExecuted = true;
                    break;
                }
            }

            foreach (var ent in argList)
                ent.Dispose();

            if (!commandExecuted)
            {
                Console.WriteLine($"Invalid command");
            }
            
            if (quit)
                Environment.Exit(0);
        }

        private void ShowHelp()
        {
            var str = "Help:";
            
            using var set = _world.GetEntities()
                .With<IsCliCommand>()
                .AsSet();
            foreach (var command in set.GetEntities())
            {
                var path = command.Get<CommandPath>().Value;
                var args = command.Get<CommandArguments>();

                str += $"\n\t{path} ";
                foreach (var output in args)
                {
                    str += $"{output.Type.Name.ToLower()}:{output.Name}";
                }

                foreach (var output in args)
                {
                    if (!string.IsNullOrEmpty(output.Description))
                    {
                        str += $"\n\t\t{output.Name}: {output.Description}";
                    }
                }
            }

            Console.WriteLine(str);

            Environment.Exit(0);
        }
    }
}
