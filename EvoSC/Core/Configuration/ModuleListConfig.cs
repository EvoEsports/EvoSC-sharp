using System.Text.Json.Serialization;

namespace EvoSC.Core.Configuration
{
    public struct ModuleListConfig
    {
        [JsonPropertyName("load")] public string[] Modules { get; set; }
    }
}
