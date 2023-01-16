using EvoSC.Common.Config.Models;

namespace EvoSC.Common.Database.Extensions;

public static class DatabaseConfigExtensions
{
    public static string GetConnectionString(this IDatabaseConfig config)
    {
        switch (config.Type)
        {
            case IDatabaseConfig.DatabaseType.PostgreSql:
                return GetPostgreSqlString(config);
            case IDatabaseConfig.DatabaseType.MySql:
                return GetMySqlString(config);
            case IDatabaseConfig.DatabaseType.SQLite:
                return GetSQLiteString(config);
            default:
                throw new InvalidOperationException("A database type has not been set.");
        }
    }

    private static string GetMySqlString(IDatabaseConfig config)
    {
        return $"Server={config.Host};Port={config.Port};Database={config.Name};Uid={config.Username};Pwd={config.Password}";
    }

    private static string GetPostgreSqlString(IDatabaseConfig config)
    {
        return $"User ID={config.Username};Password={config.Password};" +
               $"Host={config.Host};Port={config.Port};Database={config.Name};";
    }

    private static string GetSQLiteString(IDatabaseConfig config)
    {
        return $"DataSource={config.Host}";
    }
}
