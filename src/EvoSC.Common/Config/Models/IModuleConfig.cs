using System.ComponentModel;
using Config.Net;

namespace EvoSC.Common.Config.Models;

public interface IModuleConfig
{
    [Description(
        "Signature verification of module's files. If enabled and verification fails, the module will not load.")]
    [Option(Alias = "requireSignatureVerification", DefaultValue = true)]
    public bool RequireSignatureVerification { get; }

    [Description("Directories to scan for external modules.")]
    [Option(Alias = "moduleDirectories", DefaultValue = new []{"modules"})]
    public string[] ModuleDirectories { get; }
    
    [Description(
        "Modules that will not be enabled on startup. A module that depends on a module in this list " +
        "is refused when it is enabled.")]
    [Option(Alias = "disabledModules", DefaultValue = new []{"ExampleModule", "FastestCpModule", "MatchRankingModule"})]
    public string[] DisabledModules { get; }
}
