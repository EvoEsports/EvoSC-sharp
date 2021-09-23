using System;
using System.Collections.Generic;
using GbxRemoteNet.XmlRpc.Packets;
using GbxRemoteNet.XmlRpc.Types;

namespace EvoSC.Core.Remote
{
    public interface IGbxStruct
    {
        XmlRpcBaseType Serialize();
        void Deserialize(ResponseMessage response);
        void Deserialize(XmlRpcBaseType xmlBaseType);

        public static T Deserialize<T>(ResponseMessage response)
            where T : struct, IGbxStruct
        {
            var result = default(T);
            result.Deserialize(response);
            return result;
        }

        public static T Deserialize<T>(XmlRpcBaseType response)
            where T : struct, IGbxStruct
        {
            var result = default(T);
            result.Deserialize(response);
            return result;
        }
    }

    public static class GbxStructExtensions
    {
        public static TWanted GetXmlRpcType<TWanted>(this ResponseMessage message)
            where TWanted : XmlRpcBaseType
        {
            // TODO: use another method than GetResponseData() to remove reflection usage
            // (it use Activator.CreateInstance as of now)
            var result = message.GetResponseData();
            if (result.GetType() != typeof(TWanted))
            {
                throw new InvalidOperationException(
                    $"Expected '{typeof(TWanted).Name}' but had '{result.GetType().Name}'"
                );
            }

            return (TWanted)result;
        }

        public static T GetField<T>(this XmlRpcStruct xmlStruct, string field)
            where T : XmlRpcBaseType
        {
            if (!xmlStruct.Fields.TryGetValue(field, out var r))
                throw new KeyNotFoundException($"Field '{field}' not found");

            return (T)r;
        }

        public static T GetField<T>(this XmlRpcStruct xmlStruct, string field, T defaultValue)
            where T : XmlRpcBaseType
        {
            if (!xmlStruct.Fields.TryGetValue(field, out var r))
            {
                return defaultValue;
            }

            return (T)r;
        }
    }
}
