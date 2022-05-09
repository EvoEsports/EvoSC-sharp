using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Storage;
using Tomlet;
using Tomlet.Models;

namespace EvoSC.Core.Configuration;

public static class Config
{
    private static readonly Dedicated s_dedicated;
    private static readonly Theme s_theme;
    private static readonly Database s_database;

    static Config()
    {
        TomlDocument document;
        try
        {
            document = TomlParser.ParseFile(@"config/server.toml");
        }
        catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException)
        {
            throw new Exception("The config directory does not exist, or the server.toml file is missing", e);
        }

        s_dedicated = TomletMain.To<Dedicated>(document);
        if (Environment.GetEnvironmentVariable("RPC_HOST") == null)
        {
            if (s_dedicated == null || s_dedicated.IsAnyNullOrEmpty())
            {
                throw new ApplicationException("The server configuration is empty or missing values");
            }
        }
        else
        {
            s_dedicated.Host = Environment.GetEnvironmentVariable("RPC_HOST");
            s_dedicated.Port = int.Parse(Environment.GetEnvironmentVariable("RPC_PORT") ?? "5000");
            s_dedicated.AdminLogin = Environment.GetEnvironmentVariable("RPC_LOGIN");
            s_dedicated.AdminPassword = Environment.GetEnvironmentVariable("RPC_PASSWORD");
        }

        s_theme = TomletMain.To<Theme>(document);
        if (s_theme == null || s_theme.IsAnyNullOrEmpty())
        {
            throw new ApplicationException("The Theme configuration is empty or missing values");
        }

        s_database = TomletMain.To<Database>(document);
        if (Environment.GetEnvironmentVariable("DB_HOST") == null)
        {
            if (s_database == null || s_database.IsAnyNullOrEmpty())
            {
                throw new ApplicationException("The database configuration is empty or missing values");
            }
        }
        else
        {
            s_database.Type = Environment.GetEnvironmentVariable("DB_TYPE");
            s_database.Host = Environment.GetEnvironmentVariable("DB_HOST");
            s_database.Port = int.Parse(Environment.GetEnvironmentVariable("DB_PORT") ?? "3306");
            s_database.DbName = Environment.GetEnvironmentVariable("DB_NAME");
            s_database.User = Environment.GetEnvironmentVariable("DB_USER");
            s_database.Password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        }
    }


    public static Dedicated GetDedicatedConfig()
    {
        return s_dedicated;
    }

    public static Theme GetTheme()
    {
        return s_theme;
    }

    public static Database GetDatabaseConfig()
    {
        return s_database;
    }
}
