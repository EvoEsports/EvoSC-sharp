using GbxRemoteNet.XmlRpc;

namespace EvoSC.Common.Util.ServerUtils;

public static class GbxRemoteUtils
{
    /// <summary>
    /// Convert a dynamic object to a native type.
    /// </summary>
    /// <param name="obj">Object to convert.</param>
    /// <typeparam name="T">The native type to convert to.</typeparam>
    /// <returns></returns>
    public static T DynamicToType<T>(dynamic obj) =>
        (T)XmlRpcTypes.ToNativeValue<T>(XmlRpcTypes.ToXmlRpcValue(obj));
}
