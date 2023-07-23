using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.MotdModule;

[PermissionGroup]
public enum MotdPermissions
{
    [Description("Set Url to fetch Motd from.")]
    SetUrl,
    [Description("Interval of the timer to fetch Motd.")]
    SetFetchInterval,
    [Description("Changes motd to be fetched from local.")]
    SetLocal,
    [Description("Opens the Motd editor for local motd.")]
    OpenMotdEdit
}
