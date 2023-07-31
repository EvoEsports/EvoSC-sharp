using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.ModuleManagerModule;

[PermissionGroup]
public enum ModuleManagerPermissions
{
    [Description("[Permission.ActivateModule]")]
    ActivateModule,
    
    [Description("[Permission.InstallModule]")]
    InstallModule,
    
    [Description("Permission.ConfigureModules")]
    ConfigureModules
}
