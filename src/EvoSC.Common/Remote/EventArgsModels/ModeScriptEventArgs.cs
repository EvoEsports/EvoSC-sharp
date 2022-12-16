using Newtonsoft.Json.Linq;

namespace EvoSC.Common.Remote.EventArgsModels;

public class ModeScriptEventArgs : EventArgs
{
    public required string Method { get; init; }
    public required JObject Args { get; init; }
}
