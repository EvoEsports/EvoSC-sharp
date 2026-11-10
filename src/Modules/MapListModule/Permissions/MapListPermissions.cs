using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.MapListModule.Permissions;

[PermissionGroup]
public enum MapListPermissions
{
    [Description("Allow adding and removing tags on maps.")]
    ManageTags,

    [Description("Allow disabling and enabling maps, and seeing disabled maps.")]
    ToggleMapEnabled
}
