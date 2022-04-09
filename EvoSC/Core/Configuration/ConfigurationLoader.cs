using System;
using System.IO;
using Tomlet;
using Tomlet.Models;

namespace EvoSC.Core.Configuration;

public static class ConfigurationLoader
{
    public static ServerConnectionConfig LoadServerConnectionConfig()
    {
        try
        {
            TomlDocument document = TomlParser.ParseFile(@"config/server.toml");
            var config = TomletMain.To<ServerConnectionConfig>(document);
            
            if (config != null && !config.IsAnyNullOrEmpty(config))
            {
                return config;
            }

            throw new ApplicationException("The server configuration is empty or missing values");
        }
        catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException)
        {
            throw new Exception("The config directory does not exist, or the server.toml file is missing", e);
        }
    }
}
