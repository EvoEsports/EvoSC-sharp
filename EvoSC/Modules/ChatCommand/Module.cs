using System;
using EvoSC.Core.Domains;
using EvoSC.Modules.ChatCommand.Systems;
using GameHost.V3;
using GameHost.V3.Injection;
using GameHost.V3.IO.Storage;
using GameHost.V3.Module;
using GameHost.V3.Utility;

namespace EvoSC.Modules.ChatCommand
{
    public class Module : HostModule
    {
        public Module(HostRunnerScope scope) : base(scope)
        {
        }

        protected override void OnInit()
        {
            TrackDomain<ServerDomain>(domain =>
            {
                var commandManager = new ChatCommandManager(domain.Scope);
                domain.Scope.Context.Register(commandManager);
                
                Disposables.AddRange(new IDisposable[]
                {
                    new ChatCommandExecuteSystem(domain.Scope), 
                    new ChatShowHelp(domain.Scope), 
                    commandManager
                });
            });
        }
        
        // This module will use the Config folder, or else the config stuff will be in
        // a folder called `EvoSC.ChatCommand`, kinda bad
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