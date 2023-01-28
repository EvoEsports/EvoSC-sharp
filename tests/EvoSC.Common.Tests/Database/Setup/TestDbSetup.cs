using EvoSC.Common.Database.Migrations;
using EvoSC.Common.Interfaces.Database;

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
}
