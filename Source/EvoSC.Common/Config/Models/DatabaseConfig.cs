using Tomlet.Attributes;

namespace EvoSC.Common.Config.Models;

public class DatabaseConfig
{
    public enum DatabaseType
    {
        MySql
    }

    [TomlPrecedingComment("The type of database to use. Available types: MySql")]
    [TomlProperty("type")] public DatabaseType Type { get; init; } = DatabaseType.MySql;
    
    [TomlPrecedingComment("Address to the MySQL database")]
    [TomlProperty("host")] public string Host { get; init; } = "127.0.0.1";

    [TomlPrecedingComment("Port of the MySQL database")]
    [TomlProperty("port")] public int Port { get; init; } = 3306;

    [TomlPrecedingComment("The name of the database")]
    [TomlProperty("name")] public string Name { get; init; } = "evosc";

    [TomlPrecedingComment("Name of the user to access the database")]
    [TomlProperty("username")] public string Username { get; init; } = "evosc";

    [TomlPrecedingComment("Password of the user to access the database")]
    [TomlProperty("password")] public string Password { get; init; } = "evosc";

    [TomlPrecedingComment("A string prefix to add to all table names")]
    [TomlProperty("tablePrefix")] public string TablePrefix { get; init; }
}
