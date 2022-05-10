using System.Reflection;
using Tomlet.Attributes;

namespace EvoSC.Core.Configuration;

public class Dedicated
{
    [TomlProperty("dedicated.host")]
    public string Host { get; set; }
    [TomlProperty("dedicated.port")]
    public int Port { get; set; }

    [TomlProperty("dedicated.login")]
    public string AdminLogin { get; set; }
    [TomlProperty("dedicated.password")]
    public string AdminPassword { get; set; }
    
}
