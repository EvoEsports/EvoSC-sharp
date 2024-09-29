using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Permissions;

[PermissionGroup]
public enum ToornamentPermissions
{
    [Description("Can start a match through toornament.")]
    StartMatch,

    [Description("Can set up the server for a match.")]
    SetupMatch,

    [Description("Can set custom points for a player.")]
    SetPoints,

    [Description("Allows to manage the server name.")]
    ServerName
}
