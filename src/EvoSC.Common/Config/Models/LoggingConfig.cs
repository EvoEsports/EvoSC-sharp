using Tomlet.Attributes;

namespace EvoSC.Common.Config.Models;

[TomlDoNotInlineObject]
public class LoggingConfig
{
    [TomlPrecedingComment("Possible values lowest to highest verbosity: none, critical, error, warning, information, debug, trace")]
    [TomlProperty("logLevel")] public string LogLevel { get; init; } = "Information";
    
    [TomlPrecedingComment("Whether to output logs to the console in JSON.")]
    [TomlProperty("useJson")] public bool UseJson { get; init; } = false;
}
