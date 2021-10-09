using System;
using EvoSC.Core.Domains;
using EvoSC.Core.Plugins;
using GameHost.V3;
using GameHost.V3.Injection;
using GameHost.V3.IO.Storage;
using GameHost.V3.Module.Storage;

namespace Plugin.Samples.Commands
{
    public class Module : PluginModuleBase
    {
        public Module(HostRunnerScope scope) : base(scope)
        {
        }

        protected override void OnInit()
        {
            base.OnInit();

            // make sure that the ChatCommand Module will be loaded
            // (normally there shouldn't be a need to do that,
            //  but we show that there is an explicit dependency)
            LoadModule(sc => new EvoSC.Modules.ChatCommand.Module(sc));
        }

        protected override void OnServerFound(ServerDomain domain)
        {
            AddSystems(
                new PingPong(domain.Scope),
                new SendGlobalMessage(domain.Scope),
                new SendPrivateMessage(domain.Scope)
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