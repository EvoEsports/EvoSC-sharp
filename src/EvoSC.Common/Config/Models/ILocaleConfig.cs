using System.ComponentModel;
using Config.Net;

namespace EvoSC.Common.Config.Models;

public interface ILocaleConfig
{
    [Description("The default display language of the controller. Must be a \"language tag\" as found here: https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c")]
    [Option(Alias = "defaultLanguage", DefaultValue = "en")]
    public string DefaultLanguage { get; }
}
