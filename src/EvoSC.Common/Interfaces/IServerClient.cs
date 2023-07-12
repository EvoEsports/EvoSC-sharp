using EvoSC.Common.Interfaces.Models;
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
    /// Send an info message to the chat.
    /// </summary>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task InfoMessageAsync(string text);
    
    /// <summary>
    /// Send an info message to a specific player.
    /// </summary>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task InfoMessageAsync(string text, IPlayer player);

    /// <summary>
    /// Send a success message to the chat.
    /// </summary>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task SuccessMessageAsync(string text);
    
    /// <summary>
    /// Send a success message to a specific player.
    /// </summary>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task SuccessMessageAsync(string text, IPlayer player);
    
    /// <summary>
    /// Send a warning message to the chat.
    /// </summary>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task WarningMessageAsync(string text);
    
    /// <summary>
    /// Send a warning message to a specific player.
    /// </summary>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task WarningMessageAsync(string text, IPlayer player);

    /// <summary>
    /// Send a error message to the chat.
    /// </summary>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task ErrorMessageAsync(string text);
    
    /// <summary>
    /// Send a error message to a specific player.
    /// </summary>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task ErrorMessageAsync(string text, IPlayer player);
}
