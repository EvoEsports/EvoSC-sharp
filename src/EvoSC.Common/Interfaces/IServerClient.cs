using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util.TextFormatting;
using GbxRemoteNet.Interfaces;

namespace EvoSC.Common.Interfaces;

public interface IServerClient
{
    /// <summary>
    /// The GBXRemote client instance.
    /// </summary>
    public IGbxRemoteClient Remote { get; }
    /// <summary>
    /// Whether the client is connected to the remote XMLRPC server or not.
    /// </summary>
    public bool Connected { get; }
    
    public IChatService Chat { get; }

    /// <summary>
    /// Start the client and set up a connection.
    /// </summary>
    /// <param name="token">Cancellation token to cancel the startup.</param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken token);
    
    /// <summary>
    /// Stop and disconnect from the server.
    /// </summary>
    /// <param name="token">Cancellation token to cancel the shutdown.</param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken token);

    /// <summary>
    /// Get the server's maps directory.
    /// </summary>
    /// <returns></returns>
    public Task<string> GetMapsDirectoryAsync();
}
