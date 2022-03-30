using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text.Json.Serialization;

namespace EvoSC.Core.Configuration;

public class ServerConnectionConfig
{
    [JsonPropertyName("host")] public string Host { get; set; }
    [JsonPropertyName("port")] public int Port { get; set; }

    [JsonPropertyName("login")] public string AdminLogin { get; set; }
    [JsonPropertyName("password")] public string AdminPassword { get; set; }

    public bool IsAnyNullOrEmpty(ServerConnectionConfig config)
    {
        foreach(PropertyInfo pi in config.GetType().GetProperties())
        {
            if(pi.PropertyType == typeof(string))
            {
                var value = (string)pi.GetValue(config);
                if(string.IsNullOrEmpty(value))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
