using System.Text.Json.Serialization;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Models;
public class PluginData
{
    [JsonPropertyName("S_UseAutoReady")]
    public bool S_UseAutoReady { get; set; } = false;
}
