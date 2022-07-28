using EvoSC.Core.Commands.Manialink.Interfaces;
using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;

namespace EvoSC.Core.Commands.Manialink;

public class ManialinkCommandContext : IManialinkCommandContext
{
    public GbxRemoteClient Client { get; }
    public IPlayer Player { get; }
    public IManiaLinkPageAnswer Answer { get; }

    public ManialinkCommandContext(GbxRemoteClient client, IManiaLinkPageAnswer answer)
    {
        Client = client;
        Player = answer.Player;
        Answer = answer;
    }
}
