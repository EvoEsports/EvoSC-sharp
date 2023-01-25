using System.ComponentModel;
using Config.Net;

namespace EvoSC.Common.Config.Models;

public interface ILoggingConfig
{
    [Description("Possible values lowest to highest verbosity: none, critical, error, warning, information, debug, trace")]
    [Option(Alias = "logLevel", DefaultValue = "information")]
    public string LogLevel { get; }
    
    [Description("Whether to output logs to the console in JSON.")]
    [Option(Alias = "useJson", DefaultValue = false)]
    public bool UseJson { get; }
}
