using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;
using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.MotdModule;

[PermissionGroup]
[Identifier(Name = "MotdPermissions")]
public enum MotdPermissions
{
    [Identifier(Name = "SetUrl"), Description("Set Url to fetch Motd from.")]
    SetUrl,
    [Identifier(Name = "SetFetchInterval"), Description("Interval of the timer to fetch Motd.")]
    SetFetchInterval
}
