using System.ComponentModel;
using Config.Net;

namespace EvoSC.Common.Config.Models;

public interface IModuleConfig
{
    [Description("Signature verification of module's files. If enabled and verification fails, the module will not load.")]
    [Option(Alias = "requireSignatureVerification", DefaultValue = true)]
    public bool RequireSignatureVerification { get; }
}
