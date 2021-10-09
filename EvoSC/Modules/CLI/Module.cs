using System;
using DefaultEcs;
using EvoSC.Modules.CLI.InteractiveSystems;
using EvoSC.Modules.CLI.Systems;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.IO.Storage;
using GameHost.V3.Module;
using GameHost.V3.Utility;

namespace EvoSC.Modules.CLI
{
    public class Module : HostModule
    {
        private readonly World _interactiveWorld;
        private readonly Scope _scope;

        public Module(HostRunnerScope scope) : base(scope)
        {
            Disposables.AddRange(new IDisposable[]
            {
                _scope = new FreeScope(new MultipleScopeContext {scope.Context, ModuleScope.Context}),
                _interactiveWorld = new World()
            });

            ModuleScope.Context.Register(_interactiveWorld);
            {
                AddInteractiveSystem(new CliCommandManager(ModuleScope));
                AddInteractiveSystem(new InteractiveRunSystem(ModuleScope));
            }
        }

        protected override void OnInit()
        {
            var executeCommandSystem = new CliExecuteCommandSystem(_scope);
            Disposables.Add(executeCommandSystem);
        }

        private void AddInteractiveSystem(AppSystem system)
        {
            Disposables.Add(system);

            ModuleScope.Context.Register(system.GetType(), system);
            Dependencies.Add(new WaitingDependency(system));
        }
        
        // This module will use the Config folder, or else the config stuff will be in
        // a folder called `EvoSC.CLI`, kinda bad
        protected override IStorage CreateDataStorage(Scope scope)
        {
            if (!scope.Context.TryGet(out IStorage executingStorage))
            {
                throw new NullReferenceException(nameof(IStorage));
            }

            return executingStorage.GetSubStorage("Config");
        }
    }
}