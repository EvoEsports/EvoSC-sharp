using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EvoSC.Modules.Remotes.GBXRemote.Structs;
using EvoSC.Utility.Remotes;
using EvoSC.Utility.Remotes.Structs;
using GbxRemoteNet;
using GbxRemoteNet.XmlRpc;
using GbxRemoteNet.XmlRpc.Packets;
using GbxRemoteNet.XmlRpc.Types;

namespace EvoSC.Modules.Remotes.GBXRemote.Impl
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
    public class ThreadedLowLevelGbxRemote : ILowLevelGbxRemote
    {
        public GbxRemoteClient Client { get; private set; }

        public ThreadedLowLevelGbxRemote(string host, int port)
        {
            Client = new GbxRemoteClient(
                host,
                port,
                new GbxRemoteClientOptions {InvokeEventOnModeScriptMethodResponse = true});
        }

        public Task<ResponseMessage> CallMethodAsync(string method, params object[] args)
        {
            return Task.Run(() => Client.CallMethodAsync(method, args));
        }

        public async Task Connect(string login, string password)
        {
            await Client.ConnectAsync();
            await Client.AuthenticateAsync(login, password);
            await Client.SetApiVersionAsync("2013-04-16");
            await Client.EnableCallbackTypeAsync();

            var thread = new Thread(async () =>
            {
                while (true)
                {
                    Thread.Sleep(2);
    
                    Task<object[]> task;
                    lock (_multiCallSynchronization)
                    {
                        if (_multiCall.MethodCalls.Count == 0)
                            continue;
                        
                        task = Client.MultiCallAsync(_multiCall);
                        _multiCallSendIndex++;
                        
                        // reset
                        _multiCall.MethodCalls.Clear();
                    }

                    var results = await task;

                    lock (_multiCallSynchronization)
                    {
                        for (var i = 0; i < results.Length; i++)
                        {
                            _tcsList[i].SetResult(results[i]);
                        }
                        
                        _tcsList.RemoveRange(0, results.Length);
                    }
                }
            });
            
            thread.Start();
        }

        private int _multiCallSendIndex;
        private object _multiCallSynchronization = new();
        private MultiCall _multiCall = new();
        private List<TaskCompletionSource<object>> _tcsList = new();

        private Task<object> AddMultiCall(string method, params object[] args)
        {
            // We can't put this inside of a Task.Run since it can block the application for a long time if the
            // user queue a lot of calls
            // We hope the Task system will be intelligent enough to put this task into another thread

            var wait = false;
            var index = _multiCallSendIndex;
            lock (_multiCallSynchronization)
            {
                if (_multiCall.MethodCalls.Count >= 400)
                    wait = true;
            }
            
            while (wait && index == _multiCallSendIndex) {}
            
            var tcs = new TaskCompletionSource<object>();
            lock (_multiCallSynchronization)
            {
                _multiCall.Add(method, args);
                _tcsList.Add(tcs);
            }

            return tcs.Task;
        }

        public Task<bool> ChatSendServerMessageAsync(string message)
        {
            return AddMultiCall("ChatSendServerMessage", message).ContinueWith(t =>
            {
                return (bool)t.Result;
            });
        }

        public Task<bool> ChatSendServerMessageToLoginAsync(string message, string playerLogins)
        {
            return AddMultiCall("ChatSendServerMessageToLogin", message, playerLogins).ContinueWith(t =>
            {
                return (bool)t.Result;
            });
        }

        public async Task<GbxPlayerInfo?> GetPlayerInfoAsync(string login)
        {
            var response = await CallMethodAsync("GetPlayerInfo", login, 0);
            return RemotePlayerInfo.Deserialize(response);
        }

        public async Task<GbxPlayerInfo[]> GetPlayerListAsync()
        {
            var response = await CallMethodAsync("GetPlayerList", -1, 0, 0);
            var xmlArray = response.GetXmlRpcType<XmlRpcArray>();
            var result = new GbxPlayerInfo[xmlArray.Values.Length];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = RemotePlayerInfo.Deserialize(xmlArray.Values[i]);
            }

            return result;
        }

        public async Task<GbxPlayerInfo?> GetMainServerPlayerInfoAsync()
        {
            var response = await CallMethodAsync("GetMainServerPlayerInfo", 0);
            return RemotePlayerInfo.Deserialize(response);
        }
    }
}