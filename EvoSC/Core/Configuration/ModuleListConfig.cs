using System.Text.Json.Serialization;

namespace EvoSC.Core.Configuration
{
    public class ModuleListConfig
    {
        [JsonPropertyName("load")] public string[] Modules { get; set; }
    }
}