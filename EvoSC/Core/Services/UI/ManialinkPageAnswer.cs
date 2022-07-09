using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;
using GbxRemoteNet.Structs;

namespace EvoSC.Core.Services.UI;

public class ManialinkPageAnswer : IManiaLinkPageAnswer
{
    public IServerPlayer Player { get; }
    public string Content => Answer;
    public int PlayerServerId { get; }
    public string Answer { get; }
    public SEntryVal[] Entries { get; }

    public ManialinkPageAnswer(IServerPlayer player, string answer, SEntryVal[] entries, int playerUid)
    {
        Player = player;
        Answer = answer;
        Entries = entries;
        PlayerServerId = playerUid;
    }
}
