using EvoSC.Common.Config.Models;

namespace EvoSC.Common.Database;

public static class DatabaseConfigExtensions
{
    public static string GetConnectionString(this DatabaseConfig config)
    {
        switch (config.Type)
        {
            case DatabaseConfig.DatabaseType.MySql:
                return $"Server={config.Host};Port={config.Port};Database={config.Name};Uid={config.Username};Pwd={config.Password}";
        }

        return "";
    }
}
