using System.Threading.Tasks;
using EvoSC.Interfaces.Messages;
using GbxRemoteNet.Structs;
using GbxRemoteNet.XmlRpc.Packets;

namespace EvoSC.Interfaces.UI;

public interface IUiService
{
    public Task OnPlayerManialinkPageAnswer(int playerUid, string login, string answer, SEntryVal[] entries);
}
