using System.Text.Json.Serialization;

namespace EvoSC.Core.Configuration
{
    public struct ServerConnectionConfig
    {
        [JsonPropertyName("host")] public string Host { get; set; }
        [JsonPropertyName("port")] public int Port { get; set; }

        [JsonPropertyName("login")] public string AdminLogin { get; set; }
        [JsonPropertyName("password")] public string AdminPassword { get; set; }
    }
}
