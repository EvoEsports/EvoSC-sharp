using System;
using EvoSC.Core.Systems;
using EvoSC.Modules.ServerConnection;
using GameHost.V3;
using GameHost.V3.Injection;
using GameHost.V3.IO.Storage;
using GameHost.V3.Module;
using GameHost.V3.Utility;

namespace EvoSC.Core
{
    /// <summary>
    /// Entry point of the controller
    /// </summary>
    public class EvoSCEntryModule : HostModule
    {
        public EvoSCEntryModule(HostRunnerScope scope) : base(scope)
        {
            Disposables.AddRange(new IDisposable[]
            {
                new SpawnConfigurationSystem(ModuleScope), 
                new LoadModulesListSystem(ModuleScope)
            });
        }

        protected override void OnInit()
        {
            LoadModule(sc => new Module(sc));
            LoadModule(sc => new Modules.ChatCommand.Module(sc));
        }

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