using EvoSC.Core.Configuration;
using EvoSC.Domain;
using GameHost.V3;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.Module;

namespace EvoSC.Modules.DatabaseConnection
{
    public class Module : HostModule
    {
        private readonly HostRunnerScope _hostScope;

        private ControllerConfig _controllerConfig;
        
        public Module(HostRunnerScope scope) : base(scope)
        {
            _hostScope = scope;
            
            Dependencies.AddRef(() => ref _controllerConfig);
        }

        protected override void OnInit()
        {
            var db = new DatabaseContext();
            
            _hostScope.Context.Register(db);
        }
    }
}
