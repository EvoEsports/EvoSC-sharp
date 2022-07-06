using EvoSC.Domain.Commands;
using EvoSC.Domain.Players;
using GbxRemoteNet;

namespace EvoSC.Core.Services.Commands;

public class CommandContext
{
    public CommandContext(Player player, Command command, GbxRemoteClient remoteClient)
    {
        Player = player;
        Command = command;
        RemoteClient = remoteClient;
    }

    public Player Player { get; }

    public Command Command { get; }

    public GbxRemoteClient RemoteClient { get; }
}
