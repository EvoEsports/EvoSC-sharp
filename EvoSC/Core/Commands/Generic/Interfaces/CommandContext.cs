using GbxRemoteNet;

namespace EvoSC.Core.Commands.Generic.Interfaces;

/// <summary>
/// Contains information about the current command context.
/// </summary>
public class CommandContext : ICommandContext
{
    /// <summary>
    /// Remote client of the TM server.
    /// </summary>
    public GbxRemoteClient Client { get; }

    public CommandContext(GbxRemoteClient client)
    {
        Client = client;
    }
}
