using System.Text.Json.Serialization;
using EvoSC.Core.Configuration;

namespace EvoSC.Core
{
    [JsonSerializable(typeof(ModuleListConfig))]
    [JsonSerializable(typeof(ServerConnectionConfig))]
    [JsonSerializable(typeof(ControllerConfig))]
    internal partial class JsonContext : JsonSerializerContext
    {
        
    }
}