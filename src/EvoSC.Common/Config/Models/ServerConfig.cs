using Tomlet.Attributes;

namespace EvoSC.Common.Config.Models;

[TomlDoNotInlineObject]
public class ServerConfig
{
    [TomlPrecedingComment("Address to the Trackmania server")]
    [TomlProperty("host")] 
    public string Host { get; init; } = "127.0.0.1";
    
    [TomlPrecedingComment("Port that the XMLRPC is listening to")]
    [TomlProperty("port")] 
    public int Port { get; init; } = 5000;
    
    [TomlPrecedingComment("Username of the super admin account")]
    [TomlProperty("username")] 
    public string Username { get; init; } = "SuperAdmin";
    
    [TomlPrecedingComment("Password of the super admin account")]
    [TomlProperty("password")] 
    public string Password { get; init; } = "SuperAdmin";

    [TomlPrecedingCommentAttribute("If enabled, the client will try to reconnect with the server every 1 second until a connection is established")]
    [TomlProperty("retryConnection")]
    public bool RetryConnection { get; init; } = true;
}
