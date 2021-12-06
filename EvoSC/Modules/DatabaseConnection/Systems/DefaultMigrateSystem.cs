using FluentMigrator.Runner;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using NLog;

namespace EvoSC.Modules.DatabaseConnection.Systems
{
    public class DefaultMigrateSystem : AppSystem
    {
        private IMigrationRunner _migrationRunner;
        private ILogger _logger;

        public DefaultMigrateSystem(Scope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _migrationRunner);
            Dependencies.AddRef(() => ref _logger);
        }
        
        protected override void OnInit()
        {
            _logger.Info("Running database migration");

            // Automatically created from TransientMigrationRunner!
            _migrationRunner.MigrateUp();
        }
    }
}
