using System;
using GameHost.V3;
using GameHost.V3.Injection;
using GameHost.V3.IO.Storage;
using GameHost.V3.Module.Storage;
using EvoSC.Core.Domains;
using EvoSC.Core.Plugins;

namespace Plugin.Samples.ReactToEvents
{
    public class Module : PluginModuleBase
    {
        public Module(HostRunnerScope scope) : base(scope)
        {
        }

        protected override void OnServerFound(ServerDomain domain)
        {
            AddSystems(
                new ChatMessage(domain.Scope),
                new PlayersConnection(domain.Scope)
            );
        }

        protected override IStorage CreateDataStorage(Scope scope)
        {
            if (!scope.Context.TryGet(out IModuleCollectionStorage collectionStorage))
                throw new InvalidOperationException("Parent Storage couldn't be found!");
            return collectionStorage.GetSubStorage(GetModuleGroupName(typeof(Plugin.Samples.Module)));
        }
    }
}