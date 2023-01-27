using EvoSC.Common.Database.Migrations;
using EvoSC.Common.Interfaces.Database;

namespace EvoSC.Common.Tests.Database.Setup;

public static class TestDbSetup
{
    public static IDbConnectionFactory CreateFullDb(string identifier)
    {
        var factory = new TestDbConnectionFactory(identifier);
        factory.GetConnection();
        TestDbMigrations.MigrateFromAssembly(factory.ConnectionString, typeof(AddPlayersTable).Assembly);

        return factory;
    }
}
