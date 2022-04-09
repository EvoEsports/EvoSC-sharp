using System.Reflection;
using Tomlet.Attributes;

namespace EvoSC.Core.Configuration;

public class ServerConnectionConfig
{
    [TomlProperty("host")]
    public string Host { get; set; }
    [TomlProperty("port")]
    public int Port { get; set; }

    [TomlProperty("login")]
    public string AdminLogin { get; set; }
    [TomlProperty("password")]
    public string AdminPassword { get; set; }

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
