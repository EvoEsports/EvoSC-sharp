using System;
using System.Threading.Tasks;
using DefaultEcs;
using GbxRemoteNet;
using GbxRemoteNet.Enums;
using GbxRemoteNet.XmlRpc.Packets;
using GbxRemoteNet.XmlRpc.Types;
using EvoSC.Core.Remote;

namespace EvoSC.Core.Remote
{
    // Encapsulate GbxRemoteClient functions to force them to be run under other threads than the caller one.
    //
    // The reason being that any call to the client on ConstrainedTaskScheduler will be processed on the domain scheduler
    // since the methods don't force themselves by default to be on another thread.
    //
    // The caller could do by themselves 'Task.Run(() => _client.***Async())' but we need to be fool proof.
    // (and making ConstrainedTaskScheduler able to automatically go on another thread is a no no)
    /// <summary>
    /// Encapsulate <see cref="GbxRemoteClient"/> and force its method to be run on other threads.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Task.Run"/> to run the methods.
    /// </remarks>
    /// <remarks>
    /// Not all methods are present.
    /// </remarks>
    public class ForceThreadedGbxRemote : IGbxRemote
    {
        private readonly World _world;
        private readonly GbxRemoteClient _client;

        public ForceThreadedGbxRemote(World world, GbxRemoteClient client)
        {
            _world = world;
            _client = client;
            _client.OnCallback += OnCallback;
        }

        private Task OnCallback(MethodCall call)
        {
            _world.Publish(new OnRemoteCallback {Method = call.Method, Arguments = call.Arguments});
            return Task.CompletedTask;
        }

        public IDisposable SubscribeAnyEvent(MessageHandler<OnRemoteCallback> callback)
        {
            return _world.Subscribe(callback);
        }

        public Task<ResponseMessage> CallMethodAsync(string method, params object[] args)
        {
            return Task.Run(() => _client.CallMethodAsync(method, args));
        }

        public Task<bool> ConnectAsync(int retries = 0, TimeSpan timeout = default)
        {
            return Task.Run(() => _client.ConnectAsync(retries, (int)timeout.TotalMilliseconds));
        }

        public Task<bool> AuthenticateAsync(string login, string password)
        {
            return Task.Run(() => _client.AuthenticateAsync(login, password));
        }

        public Task<bool> SetApiVersionAsync(string version = "2013-04-16")
        {
            return Task.Run(() => _client.SetApiVersionAsync(version));
        }

        public Task EnableCallbackTypeAsync(
            CallbackType callbackType = CallbackType.Internal | CallbackType.ModeScript | CallbackType.Checkpoints
        )
        {
            return Task.Run(() => _client.EnableCallbackTypeAsync(callbackType));
        }

        public Task<bool> ChatSendServerMessageAsync(string message)
        {
            return Task.Run(() => _client.ChatSendServerMessageAsync(message));
        }

        public Task<bool> ChatSendServerMessageToLoginAsync(string message, string playerLogins)
        {
            return Task.Run(() => _client.ChatSendServerMessageToLoginAsync(message, playerLogins));
        }

        public async Task<GbxPlayerInfo> GetPlayerInfoAsync(string login)
        {
            return IGbxStruct.Deserialize<GbxPlayerInfo>(await CallMethodAsync("GetPlayerInfo", login, 0));
        }

        public async Task<GbxPlayerInfo[]> GetPlayerListAsync()
        {
            var response = await CallMethodAsync("GetPlayerList", -1, 0, 0);
            var xmlArray = response.GetXmlRpcType<XmlRpcArray>();
            var result = new GbxPlayerInfo[xmlArray.Values.Length];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = IGbxStruct.Deserialize<GbxPlayerInfo>(xmlArray.Values[i]);
            }

            return result;
        }

        public async Task<GbxPlayerInfo> GetMainServerPlayerInfoAsync()
        {
            return IGbxStruct.Deserialize<GbxPlayerInfo>(await CallMethodAsync("GetMainServerPlayerInfo", 0));
        }
    }
}
