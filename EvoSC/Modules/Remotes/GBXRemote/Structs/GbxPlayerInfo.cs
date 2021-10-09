using EvoSC.Utility.Remotes.Structs;
using GbxRemoteNet.XmlRpc.ExtraTypes;
using GbxRemoteNet.XmlRpc.Packets;
using GbxRemoteNet.XmlRpc.Types;

namespace EvoSC.Modules.Remotes.GBXRemote.Structs
{
    internal struct RemotePlayerInfo
    {
        public static XmlRpcBaseType Serialize(GbxPlayerInfo self)
        {
            var fields = new Struct
            {
                {nameof(self.Login), new XmlRpcString(self.Login)},
                {nameof(self.NickName), new XmlRpcString(self.NickName)},
                {nameof(self.PlayerId), new XmlRpcInteger(self.PlayerId)},
                {nameof(self.TeamId), new XmlRpcInteger(self.TeamId)},
                {nameof(self.SpectatorStatus), new XmlRpcInteger(self.SpectatorStatus)},
                {nameof(self.LadderRanking), new XmlRpcInteger(self.LadderRanking)},
                {nameof(self.Flags), new XmlRpcInteger(self.Flags)},
            };

            return new XmlRpcStruct(fields);
        }

        public static GbxPlayerInfo Deserialize(ResponseMessage response)
        {
            return Deserialize(response.GetXmlRpcType<XmlRpcStruct>());
        }

        public static GbxPlayerInfo Deserialize(XmlRpcBaseType xmlBaseType)
        {
            var xml = (XmlRpcStruct)xmlBaseType;

            return new GbxPlayerInfo
            {
                Login = xml.GetField<XmlRpcString>(nameof(GbxPlayerInfo.Login)).Value,
                NickName = xml.GetField<XmlRpcString>(nameof(GbxPlayerInfo.NickName)).Value,
                PlayerId = xml.GetField<XmlRpcInteger>(nameof(GbxPlayerInfo.PlayerId)).Value,
                TeamId = xml.GetField<XmlRpcInteger>(nameof(GbxPlayerInfo.TeamId)).Value,
                LadderRanking = xml.GetField<XmlRpcInteger>(nameof(GbxPlayerInfo.LadderRanking)).Value,

                // SpectatorStatus and Flags seems to not be received sometimes, so we add a default value
                SpectatorStatus = xml.GetField(
                    nameof(GbxPlayerInfo.SpectatorStatus), new XmlRpcInteger(0)
                ).Value,
                Flags = xml.GetField(
                    nameof(GbxPlayerInfo.Flags), new XmlRpcInteger(0)
                ).Value
            };
        }
    }
}