using System;
using System.Threading.Tasks;
using DefaultEcs;
using GbxRemoteNet.Enums;
using GbxRemoteNet.XmlRpc.Packets;
using GbxRemoteNet.XmlRpc.Types;
using EvoSC.Core.Remote;

namespace EvoSC.Core.Remote
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
    public interface IGbxRemote
    {
        // TODO: document methods
        
        IDisposable SubscribeAnyEvent(MessageHandler<OnRemoteCallback> callback);
        
        Task<ResponseMessage> CallMethodAsync(string method, params object[] args);

        Task<bool> ConnectAsync(int retries = 0, TimeSpan timeout = default);
        Task<bool> AuthenticateAsync(string login, string password);
        Task<bool> SetApiVersionAsync(string version = "2013-04-16");
        Task EnableCallbackTypeAsync(CallbackType callbackType = CallbackType.ModeScript | CallbackType.Checkpoints);

        Task<bool> ChatSendServerMessageAsync(string message);
        Task<bool> ChatSendServerMessageToLoginAsync(string message, string playerLogins);
        Task<GbxPlayerInfo> GetPlayerInfoAsync(string login);
        Task<GbxPlayerInfo[]> GetPlayerListAsync();
        Task<GbxPlayerInfo> GetMainServerPlayerInfoAsync();
    }
}
