using System.ComponentModel;
using Config.Net;

namespace EvoSC.Common.Config.Models;

public interface IServerConfig
{
    [Description("Address to the Trackmania server")]
    [Option(Alias = "host", DefaultValue = "172.23.97.27")]
    public string Host { get; }
    
    [Description("Port that the XMLRPC is listening to")]
    [Option(Alias = "port", DefaultValue = 5000)]
    public int Port { get; }
    
    [Description("Username of the super admin account")]
    [Option(Alias = "username", DefaultValue = "SuperAdmin")]
    public string Username { get; }
    
    [Description("Password of the super admin account")]
    [Option(Alias = "password", DefaultValue = "SuperAdmin")]
    public string Password { get; }

    [Description("If enabled, the client will try to reconnect with the server every 1 second until a connection is established")]
    [Option(Alias = "retryConnection", DefaultValue = true)]
    public bool RetryConnection { get; }
}
