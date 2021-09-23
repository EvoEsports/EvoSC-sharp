using GbxRemoteNet.XmlRpc.ExtraTypes;
using GbxRemoteNet.XmlRpc.Packets;
using GbxRemoteNet.XmlRpc.Types;

namespace EvoSC.Core.Remote
{
    public struct GbxPlayerInfo : IGbxStruct
    {
        public string Login;
        public string NickName;
        public int PlayerId;
        public int TeamId;
        public int SpectatorStatus;
        public int LadderRanking;
        public int Flags;

        public XmlRpcBaseType Serialize()
        {
            var fields = new Struct
            {
                {nameof(Login), new XmlRpcString(Login)},
                {nameof(NickName), new XmlRpcString(NickName)},
                {nameof(PlayerId), new XmlRpcInteger(PlayerId)},
                {nameof(TeamId), new XmlRpcInteger(TeamId)},
                {nameof(SpectatorStatus), new XmlRpcInteger(SpectatorStatus)},
                {nameof(LadderRanking), new XmlRpcInteger(LadderRanking)},
                {nameof(Flags), new XmlRpcInteger(Flags)},
            };

            return new XmlRpcStruct(fields);
        }

        public void Deserialize(ResponseMessage response)
        {
            Deserialize(response.GetXmlRpcType<XmlRpcStruct>());
        }

        public void Deserialize(XmlRpcBaseType xmlBaseType)
        {
            var xml = (XmlRpcStruct)xmlBaseType;

            Login = xml.GetField<XmlRpcString>(nameof(Login)).Value;
            NickName = xml.GetField<XmlRpcString>(nameof(NickName)).Value;
            PlayerId = xml.GetField<XmlRpcInteger>(nameof(PlayerId)).Value;
            TeamId = xml.GetField<XmlRpcInteger>(nameof(TeamId)).Value;
            LadderRanking = xml.GetField<XmlRpcInteger>(nameof(LadderRanking)).Value;
            
            // SpectatorStatus and Flags seems to not be received sometimes, so we add a default value
            SpectatorStatus = xml.GetField(nameof(SpectatorStatus), new XmlRpcInteger(0)).Value;
            Flags = xml.GetField(nameof(Flags), new XmlRpcInteger(0)).Value;
        }
    }
}
