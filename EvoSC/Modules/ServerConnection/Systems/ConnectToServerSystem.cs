using System.Threading.Tasks;
using EvoSC.Core.Configuration;
using EvoSC.Utility.Remotes;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.IO.Events;
using NLog;

namespace EvoSC.Modules.ServerConnection
{
    public class ConnectToServerSystem : AppSystem
    {
        private readonly Bindable<ServerConnectionState> _stateBindable;
        private ServerConnectionConfig _connectionConfig;
        private ILowLevelGbxRemote _remoteClient;

        private ILogger _logger;

        public ConnectToServerSystem(Scope scope) : base(scope)
        {
            _stateBindable = new Bindable<ServerConnectionState>();

            scope.Context.Register(new ServerConnectionStateBindable(_stateBindable));

            Dependencies.AddRef(() => ref _logger);
            Dependencies.AddRef(() => ref _remoteClient);
            Dependencies.AddRef(() => ref _connectionConfig);
        }

        protected override async void OnInit()
        {
            _logger.Info("Connect to {0}:{1}", _connectionConfig.Host, _connectionConfig.Port);

            _stateBindable.Value = ServerConnectionState.Disconnected;

            var task = Task.Run(() => _remoteClient.Connect(
                _connectionConfig.AdminLogin,
                _connectionConfig.AdminPassword
            ));

            _stateBindable.Value = ServerConnectionState.Connecting;
            await task;

            _stateBindable.Value = ServerConnectionState.Connected;
        }
    }
}