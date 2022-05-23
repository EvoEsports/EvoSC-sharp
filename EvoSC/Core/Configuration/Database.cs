using System;
using System.Linq;
using System.Reflection;
using Tomlet.Attributes;

namespace EvoSC.Core.Configuration;

public class Database
{

    [TomlProperty("database.type")]
    public string Type { get; set; }

    [TomlProperty("database.host")]
    public string Host { get; set; }

    [TomlProperty("database.port")]
    public int Port { get; set; }

    [TomlProperty("database.user")]
    public string User { get; set; }

    [TomlProperty("database.password")]
    public string Password { get; set; }

    [TomlProperty("database.dbname")]
    public string DbName { get; set; }

    public string GetConnectionString()
    {
        return $"server={Host};port={Port};uid={User};password={Password};database={DbName}";
    }
}
