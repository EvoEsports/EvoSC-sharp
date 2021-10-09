using GameHost.V3;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.Module;
using NLog;

namespace Plugin.Samples
{
    public class Module : HostModule
    {
        private ILogger _logger;
        
        public Module(HostRunnerScope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _logger);
        }

        protected override void OnInit()
        {
            _logger.Info("Plugin.Samples loaded");
            
            LoadModule(sc => new Simple.Module(sc));
            LoadModule(sc => new ReactToEvents.Module(sc));
            LoadModule(sc => new Commands.Module(sc));
        }

        protected override void OnDispose()
        {
            _logger.Info("Plugin.Samples is unloading kek");
            
            base.OnDispose();
        }
    }
}