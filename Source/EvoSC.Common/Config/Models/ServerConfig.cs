using Tomlet.Attributes;

namespace EvoSC.Common.Config.Models;

[TomlDoNotInlineObject]
public class ServerConfig
{
    [TomlProperty("host")] public string Host { get; } = "127.0.0.1";
    [TomlProperty("port")] public int Port { get; } = 5000;
    [TomlProperty("username")] public string Username { get; } = "SuperAdmin";
    [TomlProperty("password")] public string Password { get;  } = "SuperAdmin";
}
