using System.Reflection;
using EvoSC.Common.Database.Migrations;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Tests.Database;
using FluentMigrator.Expressions;

namespace EvoSC.Testing.Database;

public static class TestDbSetup
{
    /// <summary>
    /// Creates a testing database with all tables.
    /// </summary>
    /// <param name="identifier">A unique identifier for this database.</param>
    /// <returns></returns>
    public static IDbConnectionFactory CreateDb(string identifier, params Assembly[] migrationAssemblies)
    {
        var factory = new TestDbConnectionFactory(identifier);
        factory.GetConnection();

        foreach (var asm in migrationAssemblies)
        {
            TestDbMigrations.MigrateFromAssembly(factory.ConnectionString, asm);
        }

        return factory;
    }

    public static IDbConnectionFactory CreateDb(params Assembly[] migrationAssemblies) => CreateDb(Guid.NewGuid().ToString(), migrationAssemblies);
}
