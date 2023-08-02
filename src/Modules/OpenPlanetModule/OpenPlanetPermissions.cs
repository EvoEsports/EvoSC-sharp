using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.OpenPlanetModule;

[PermissionGroup]
public enum OpenPlanetPermissions
{
    [Description("[Permission.CanBypassVerification]")]
    CanBypassVerification,
    
    [Description("[Permission.CanUseControlPanel]")]
    CanUseControlPanel
}
