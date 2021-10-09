using System;
using System.Diagnostics;
using System.IO;
using DefaultEcs;
using EvoSC.Core;
using EvoSC.Modules.CLI.Systems;
using EvoSC.Utility.Commands;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.IO.Storage;
using GameHost.V3.Module;
using GameHost.V3.Module.Storage;

namespace EvoSC.Modules.CLI.InteractiveSystems
{
    public class InteractiveRunSystem : AppSystem
    {
        private CliCommandManager _commandMgr;
        private HostRunnerScope _hostScope;

        public InteractiveRunSystem(Scope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _commandMgr);
            Dependencies.AddRef(() => ref _hostScope);
        }

        protected override void OnInit()
        {
            _commandMgr.Add(
                "run",
                OnRun,
                "Run EvoSC in the current terminal process"
            );
            
            _commandMgr.Add(
                "run",
                OnRun,
                "Run EvoSC in the current terminal process",
                new[]
                {
                    new CommandArguments.Argument(typeof(string), "folder", "Directory where modules, config are located")
                }
            );

            _commandMgr.Add(
                "run-daemon",
                OnRunDaemon,
                "Run EvoSC in a new process"
            );
        }

        private bool OnRun(Span<Entity> args)
        {
            if (args.Length == 1)
            {
                var folder = args[0].Get<string>();
                if (!Directory.Exists(folder))
                    throw new DirectoryNotFoundException(folder);
                
                _hostScope.UserStorage = new LocalStorage(folder);
                _hostScope.ModuleStorage = new ModuleCollectionStorage(_hostScope.UserStorage.GetSubStorage("Modules"));
            }
            
            HostModule.LoadModule(_hostScope, (sc) => new EvoSCEntryModule(sc));
            return true;
        }

        private bool OnRunDaemon(Span<Entity> args)
        {
            var start = Environment.CommandLine.Split()[0];
            var process = Process.Start(new ProcessStartInfo($"dotnet", $"{start} run")
            {
                UseShellExecute = true
            });
            if (process == null)
                throw new InvalidOperationException("couldn't run self!");
            
            Console.WriteLine($"{process.Id}");
            return false;
        }
    }
}