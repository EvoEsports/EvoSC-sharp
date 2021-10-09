using System;
using EvoSC.Core.Domains;
using EvoSC.Modules.Remotes.GBXRemote.Impl;
using EvoSC.Utility.Remotes;
using GameHost.V3;
using GameHost.V3.Utility;
using CreateEventServerSystem = EvoSC.Modules.Remotes.GBXRemote.Systems.CreateEventServerSystem;

namespace EvoSC.Modules.Remotes.GBXRemote
{
    public class Module : RemoteModuleBase
    {
        public Module(HostRunnerScope scope) : base(scope)
        {
        }

        protected override ILowLevelGbxRemote OnCreateRemote(ServerDomain domain)
        {
            Disposables.AddRange(new IDisposable[] {new CreateEventServerSystem(domain.Scope)});

            return new ThreadedLowLevelGbxRemote(
                domain.ConnectionConfig.Host,
                domain.ConnectionConfig.Port
            );
        }
    }
}