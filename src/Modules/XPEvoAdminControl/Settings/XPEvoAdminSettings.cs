using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.XPEvoAdminControl.Settings;

[Settings]
public interface XPEvoAdminSettings
{
    [Option(DefaultValue = "test"), Description("The access token required to access this server.")]
    public string AccessToken { get; set; }
}
