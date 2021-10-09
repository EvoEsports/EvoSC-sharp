using System;
using System.Net;
using System.Threading.Tasks;
using EvoSC.Utility.Remotes;
using EvoSC.Utility.Remotes.Structs;
using UnsafeGbxConnector;
using UnsafeGbxConnector.Serialization.Writers;

namespace EvoSC.Modules.Remotes.UnsafeGbxConnector.Impl
{
    public partial class UnsafeLowLevelGbxRemote : ILowLevelGbxRemote
    {
        public readonly GbxConnection Client;

        private readonly IPEndPoint _endPoint;

        public UnsafeLowLevelGbxRemote(IPEndPoint endPoint)
        {
            Client = new GbxConnection();
            _endPoint = endPoint;
        }

        private bool isConnected;

        public async Task Connect(string login, string password)
        {
            Client.Connect(_endPoint);

            var writer = new GbxWriter("Authenticate");
            writer.WriteString(login);
            writer.WriteString(password);

            if (!await Client.QueueAsync(writer, s_BooleanResult))
                throw new InvalidOperationException("Couldn't authenticate with current configuration");

            isConnected = true;

            writer = new GbxWriter("SetApiVersion");
            writer.WriteString("2013-04-16");

            if (!await Client.QueueAsync(writer, s_BooleanResult))
                throw new InvalidOperationException("Couldn't set API version");

            writer = new GbxWriter("EnableCallbacks");
            writer.WriteBool(true);
            if (!await Client.QueueAsync(writer, s_BooleanResult))
                throw new InvalidOperationException("Couldn't enable callbacks");

            writer = new GbxWriter("TriggerModeScriptEventArray");
            writer.WriteString("XmlRpc.EnableCallbacks");
            using (var array = writer.BeginArray())
            {
                array.AddString("1");
            }

            if (!await Client.QueueAsync(writer, s_BooleanResult))
                throw new InvalidOperationException("Couldn't enable mode script callbacks");
        }

        public Task<bool> ChatSendServerMessageAsync(string message)
        {
            var writer = new GbxWriter("ChatSendServerMessage");
            writer.WriteString(message);

            return Client.QueueAsync(writer, s_BooleanResult);
        }

        public Task<bool> ChatSendServerMessageToLoginAsync(string message, string playerLogins)
        {
            var writer = new GbxWriter("ChatSendServerMessageToLogin");
            writer.WriteString(message);
            writer.WriteString(playerLogins);

            return Client.QueueAsync(writer, s_BooleanResult);
        }

        public Task<GbxPlayerInfo?> GetPlayerInfoAsync(string login)
        {
            var writer = new GbxWriter("GetPlayerInfo");
            writer.WriteString(login);

            return Client.QueueAsync(writer, s_GetPlayerInfoResult);
        }

        public Task<GbxPlayerInfo[]> GetPlayerListAsync()
        {
            var writer = new GbxWriter("GetPlayerList");
            writer.WriteInt(-1);
            writer.WriteInt(0);
            writer.WriteInt(0);

            return Client.QueueAsync(writer, s_GetPlayerInfoArrayResult);
        }

        public Task<GbxPlayerInfo?> GetMainServerPlayerInfoAsync()
        {
            var writer = new GbxWriter("GetMainServerPlayerInfo");
            writer.WriteInt(0);

            return Client.QueueAsync(writer, s_GetPlayerInfoResult);
        }
    }
}
