using System;
using EvoSC.Core.Systems;
using GameHost.V3;
using GameHost.V3.Injection;
using GameHost.V3.IO.Storage;
using GameHost.V3.Module;
using GameHost.V3.Utility;

namespace EvoSC.Core
{
    public class EvoSCEntryModule : HostModule
    {
        public EvoSCEntryModule(HostRunnerScope scope) : base(scope)
        {
        }

        protected override void OnInit()
        {
            LoadModule(sc => new ServerConnection.Module(sc));
            LoadModule(sc => new ChatCommand.Module(sc));

            Disposables.AddRange(new IDisposable[]
            {
                new SpawnConfigurationSystem(ModuleScope), 
                new LoadModulesListSystem(ModuleScope)
            });
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
