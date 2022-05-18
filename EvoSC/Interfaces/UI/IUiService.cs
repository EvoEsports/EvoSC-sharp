using System.Threading.Tasks;
using GbxRemoteNet.XmlRpc.Packets;

namespace EvoSC.Interfaces.UI;

public interface IUiService
{
    public Task OnAnyCallback(MethodCall call, object[] param);
    
}
