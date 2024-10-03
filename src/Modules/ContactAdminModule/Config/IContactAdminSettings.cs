using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Nsgr.ContactAdminModule.Config;

[Settings]
public interface IContactAdminSettings
{
    [Option(DefaultValue = ""), Description("Specifies the Discord Webhook endpoint to POST to.")]
    public string WebhookURL { get; set; }
}
