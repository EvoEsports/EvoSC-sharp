using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Nsgr.ContactAdminModule.Config;

[Settings]
public interface IContactAdminSettings
{
    [Option(DefaultValue = ""), Description("Specifies the Discord Webhook endpoint to POST to.")]
    public string WebhookUrl { get; set; }

    [Option(DefaultValue = ""), Description("A suffix that will be added to each message. Can be used for Discord pings")]
    public string MessageSuffix { get; set; }
}
