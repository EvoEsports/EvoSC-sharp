using System;
using System.IO;
using Tomlet;
using Tomlet.Models;

namespace EvoSC.Core.Configuration;

public static class ConfigurationLoader
{
    private static Theme s_theme;
    public static ServerConnectionConfig LoadServerConnectionConfig()
    {
        try
        {
            TomlDocument document = TomlParser.ParseFile(@"config/server.toml");
            var config = TomletMain.To<ServerConnectionConfig>(document);

            if (config == null || config.IsAnyNullOrEmpty(config))
            {
                throw new ApplicationException("The server configuration is empty or missing values");
            }

            return config;
        }
        catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException)
        {
            throw new Exception("The config directory does not exist, or the server.toml file is missing", e);
        }
    }

    public static Theme LoadTheme()
    {
        try
        {
            TomlDocument document = TomlParser.ParseFile(@"config/server.toml");
            Theme config = TomletMain.To<Theme>(document);

            if (config == null || config.IsAnyNullOrEmpty(config))
            {
                throw new ApplicationException("The theme configuration is empty or missing values");
            }

            s_theme = config;
            return config;
        }
        catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException)
        {
            throw new Exception("The config directory does not exist, or the server.toml file is missing", e);
        }
    }

    public static Database LoadDatabaseConfig()
    {
        try
        {
            TomlDocument document = TomlParser.ParseFile(@"config/server.toml");
            Database config = TomletMain.To<Database>(document);

            if (config == null || config.IsAnyNullOrEmpty(config))
            {
                throw new ApplicationException("The database configuration is empty or missing values");
            }

            return config;
        }
        catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException)
        {
            throw new Exception("The config directory does not exist, or the server.toml file is missing", e);
        }
    }

    public static Theme GetTheme()
    {
        return s_theme;
    }
}
