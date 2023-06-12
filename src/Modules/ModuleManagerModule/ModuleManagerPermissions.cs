using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.ModuleManagerModule;

[PermissionGroup]
public enum ModuleManagerPermissions
{
    [Description("Can enable or disable modules.")]
    ActivateModule,
    
    [Description("Can install or uninstall modules.")]
    InstallModule,
    
    [Description("Can access all modules's configuration.")]
    ConfigureModules
}
