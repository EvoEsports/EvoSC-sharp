using System;
using EvoSC.Common.Database.Migrations;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Modules.Official.MotdModule.Database.Migrations;

namespace EvoSC.Common.Tests.Database.Setup;

public static class TestDbSetup
{
    /// <summary>
    /// Creates a testing database with all tables.
    /// </summary>
    /// <param name="identifier">A unique identifier for this database.</param>
    /// <returns></returns>
    public static IDbConnectionFactory CreateFullDb(string identifier)
    {
        var factory = new TestDbConnectionFactory(identifier);
        factory.GetConnection();
        TestDbMigrations.MigrateFromAssembly(factory.ConnectionString, typeof(AddPlayersTable).Assembly);

        return factory;
    }

    public static IDbConnectionFactory CreateFullDb() => CreateFullDb(Guid.NewGuid().ToString());
}
