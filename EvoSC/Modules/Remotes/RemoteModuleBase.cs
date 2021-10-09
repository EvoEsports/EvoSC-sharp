using EvoSC.Core.Domains;
using EvoSC.Utility.Remotes;
using GameHost.V3;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.Module;
using NLog;

namespace EvoSC.Modules.Remotes
{
    public abstract class RemoteModuleBase : HostModule
    {
        private ILogger _logger;

        protected RemoteModuleBase(HostRunnerScope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _logger);
        }

        protected override void OnInit()
        {
            TrackDomain<ServerDomain>(domain =>
            {
                var remote = OnCreateRemote(domain);
                domain.Scope.Context.Register(remote);

                if (_logger.IsTraceEnabled)
                    _logger.Trace($"Remote '{remote.GetType()}' configured.");
            });
        }

        protected abstract ILowLevelGbxRemote OnCreateRemote(ServerDomain serverDomain);
    }
}
