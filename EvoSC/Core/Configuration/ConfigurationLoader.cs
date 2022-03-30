using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EvoSC.Core.Configuration;

public static class ConfigurationLoader
{
    public static ServerConnectionConfig LoadServerConnectionConfig()
    {
        try
        {
            using var reader = File.OpenText(@"config/server.json");
            var o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
            var config = o.ToObject<ServerConnectionConfig>();
            if (config != null && !config.IsAnyNullOrEmpty(config))
            {
                return config;
            }

            throw new ApplicationException("The server configuration is empty or missing values");
        }
        catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException)
        {
            throw new Exception("The config directory does not exist, or the server.json file is missing", e);
        }
    }
}
