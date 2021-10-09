using System.Threading.Tasks;
using EvoSC.Utility.Remotes.Structs;
using GbxRemoteNet.XmlRpc.Types;

#nullable enable

namespace EvoSC.Utility.Remotes
{
    public struct OnRemoteCallback
    {
        public string Method;
        public XmlRpcBaseType[] Arguments;
    }

// - Can be used for mocking in unit tests

    /// <summary>
    /// Interface for a gbx remote.
    /// </summary>
    public interface ILowLevelGbxRemote
    {
        Task Connect(string login, string password);

        Task<bool> ChatSendServerMessageAsync(string message);
        Task<bool> ChatSendServerMessageToLoginAsync(string message, string playerLogins);
        Task<GbxPlayerInfo?> GetPlayerInfoAsync(string login);
        Task<GbxPlayerInfo[]?> GetPlayerListAsync();
        Task<GbxPlayerInfo?> GetMainServerPlayerInfoAsync();
    }
}
