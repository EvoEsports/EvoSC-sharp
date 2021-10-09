using System;
using System.Net;
using EvoSC.Core.Domains;
using EvoSC.Modules.Remotes.UnsafeGbxConnector.Impl;
using EvoSC.Modules.Remotes.UnsafeGbxConnector.Systems;
using EvoSC.Utility.Remotes;
using GameHost.V3;
using GameHost.V3.Utility;

namespace EvoSC.Modules.Remotes.UnsafeGbxConnector
{
    public class Module : RemoteModuleBase
    {
        public Module(HostRunnerScope scope) : base(scope)
        {
        }

        protected override ILowLevelGbxRemote OnCreateRemote(ServerDomain domain)
        {
            Disposables.AddRange(new IDisposable[] {new CreateEventServerSystem(domain.Scope)});

            return new UnsafeLowLevelGbxRemote(
                new IPEndPoint(
                    IPAddress.Parse(domain.Scope.ConnectionConfig.Host),
                    domain.Scope.ConnectionConfig.Port
                )
            );
        }
    }
}
