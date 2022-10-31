using GbxRemoteNet;

namespace EvoSC.Common.Interfaces;

public interface IServerClient
{
    /// <summary>
    /// The GBXRemote client instance.
    /// </summary>
    public GbxRemoteClient Remote { get; }
    public bool Connected { get; }

    /// <summary>
    /// Start the client and set up a connection.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken token);
    /// <summary>
    /// Stop and disconnect from the server.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken token);
}
