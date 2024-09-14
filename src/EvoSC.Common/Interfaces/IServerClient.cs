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

    /* /// <summary>
    /// Send an info message to the chat.
    /// </summary>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task InfoMessageAsync(string text);
    
    /// <summary>
    /// Send an info message to a specific player.
    /// </summary>
    /// <param name="player">Player to send the message to.</param>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task InfoMessageAsync(IPlayer player, string text);

    /// <summary>
    /// Send a success message to the chat.
    /// </summary>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task SuccessMessageAsync(string text);
    
    /// <summary>
    /// Send a success message to a specific player.
    /// </summary>
    /// <param name="player">Player to send the message to.</param>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task SuccessMessageAsync(IPlayer player, string text);
    
    /// <summary>
    /// Send a warning message to the chat.
    /// </summary>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task WarningMessageAsync(string text);
    
    /// <summary>
    /// Send a warning message to a specific player.
    /// </summary>
    /// <param name="player">Player to send the message to.</param>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task WarningMessageAsync(IPlayer player, string text);

    /// <summary>
    /// Send a error message to the chat.
    /// </summary>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task ErrorMessageAsync(string text);
    
    
    /// <summary>
    /// Send a error message to a specific player.
    /// </summary>
    /// <param name="player">Player to send the message to.</param>
    /// <param name="text">Text to send.</param>
    /// <returns></returns>
    public Task ErrorMessageAsync(IPlayer player, string text);
 */
    public Task<string> GetMapsDirectoryAsync();
    
    public Task<bool> FileExistsAsync(string file);
}
