using System.ComponentModel;
using Config.Net;
using Tomlet.Attributes;

namespace EvoSC.Common.Config.Models;

public interface IDatabaseConfig
{
    /// <summary>
    /// Database types supported by the application.
    /// </summary>
    public enum DatabaseType
    {
        MySql
    }

    [Description("The type of database to use. Available types: MySql")]
    [Option(Alias = "type", DefaultValue = DatabaseType.MySql)]
    public DatabaseType Type { get; }
    
    [Description("Address to the MySQL database")]
    [Option(Alias = "host", DefaultValue = "127.0.0.1")]
    public string Host { get; }

    [Description("Port of the MySQL database")]
    [Option(Alias = "port", DefaultValue = 3306)]
    public int Port { get; }

    [Description("The name of the database")]
    [Option(Alias = "name", DefaultValue = "evosc")]
    public string Name { get; }

    [Description("Name of the user to access the database")]
    [Option(Alias = "username", DefaultValue = "evosc")]
    public string Username { get; }

    [Description("Password of the user to access the database")]
    [Option(Alias = "password", DefaultValue = "evosc")]
    public string Password { get; }

    [Description("A string prefix to add to all table names")]
    [Option(Alias = "tablePrefix")]
    public string TablePrefix { get; }
}
