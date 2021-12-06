using System;
using System.Reflection;
using FluentMigrator.Runner;
using GameHost.V3.Injection;

namespace EvoSC.Modules.DatabaseConnection.Injections
{
    public class TransientMigrationRunner : DynamicDependency<IMigrationRunner>
    {
        private readonly Func<Assembly, IMigrationRunner> _getRunner;

        public TransientMigrationRunner(Func<Assembly, IMigrationRunner> getRunner)
        {
            this._getRunner = getRunner;
        }
        
        // Get the Runner from the provided 'getRunner' function.
        // The 'assembly' argument will be given from the caller.
        public override IMigrationRunner CreateT<TContext>(TContext context)
        {
            // The caller is 'sometime' injected as 'object'
            // This apply by default for inherited AppSystem and HostModule.
            if (!context.TryGet(typeof(object), out var caller))
            {
                throw new InvalidOperationException("Caller not found");
            }

            return _getRunner(caller.GetType().Assembly);
        }
    }
}
