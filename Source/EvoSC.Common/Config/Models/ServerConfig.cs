using Tomlet.Attributes;

namespace EvoSC.Common.Config.Models;

[TomlDoNotInlineObject]
public class ServerConfig
{
    [TomlPrecedingComment("Address to the Trackmania server")]
    [TomlProperty("host")] public string Host { get; init; } = "127.0.0.1";
    
    [TomlPrecedingComment("Port that the XMLRPC is listening to")]
    [TomlProperty("port")] public int Port { get; init; } = 5000;
    
    [TomlPrecedingComment("Username of the super admin account")]
    [TomlProperty("username")] public string Username { get; init; } = "SuperAdmin";
    
    [TomlPrecedingComment("Password of the super admin account")]
    [TomlProperty("password")] public string Password { get; init; } = "SuperAdmin";
}
