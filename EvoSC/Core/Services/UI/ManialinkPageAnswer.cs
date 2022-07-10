using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;
using GbxRemoteNet.Structs;
using Newtonsoft.Json.Linq;

namespace EvoSC.Core.Services.UI;

public class ManialinkPageAnswer : IManiaLinkPageAnswer
{
    public IServerPlayer Player { get; }
    public string Content { get; }
    public int PlayerServerId { get; }
    public SEntryVal[] Entries { get; }

    public ManialinkPageAnswer(IServerPlayer player, string answer, SEntryVal[] entries, int playerUid)
    {
        Player = player;
        Content = answer;
        Entries = entries;
        PlayerServerId = playerUid;
    }
}
