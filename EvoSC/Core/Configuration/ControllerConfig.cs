using System.Text.Json.Serialization;

namespace EvoSC.Core.Configuration
{
    public class ControllerConfig
    {
        /// <summary>
        /// Indicate which remote type to utilize
        /// </summary>
        /// <remarks>
        /// If empty, it will use a GBXRemote.Net based GbxRemoteClient
        /// </remarks>
        [JsonPropertyName("remote")] public string RemoteModule { get; set; }
    }
}
