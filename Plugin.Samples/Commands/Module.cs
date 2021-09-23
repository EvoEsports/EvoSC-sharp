using System;
using GameHost.V3;
using GameHost.V3.Injection;
using GameHost.V3.IO.Storage;
using GameHost.V3.Module.Storage;
using Plugin.Samples.Commands;
using EvoSC.ChatCommand;
using EvoSC.Core.Domains;
using EvoSC.Core.Plugins;

namespace Plugin.Samples.SimpleCommand
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
            LoadModule(sc => new EvoSC.ChatCommand.Module(sc));
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