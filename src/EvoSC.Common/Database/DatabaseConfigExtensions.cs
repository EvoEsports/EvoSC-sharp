using EvoSC.Common.Config.Models;

namespace EvoSC.Common.Database;

public static class DatabaseConfigExtensions
{
    public static string GetConnectionString(this IDatabaseConfig config)
    {
        switch (config.Type)
        {
            case IDatabaseConfig.DatabaseType.MySql:
                return GetMySqlString(config);
            default:
                return "";
        }

        return "";
    }

    private static string GetMySqlString(IDatabaseConfig config)
    {
        return $"Server={config.Host};Port={config.Port};Database={config.Name};Uid={config.Username};Pwd={config.Password}";
    }
}
