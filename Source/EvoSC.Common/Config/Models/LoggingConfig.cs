using Tomlet.Attributes;

namespace EvoSC.Common.Config.Models;

[TomlDoNotInlineObject]
public class LoggingConfig
{
    [TomlProperty("logLevel")] public string LogLevel { get; } = "Information";
    [TomlProperty("useJson")] public bool UseJson { get; } = false;
}
