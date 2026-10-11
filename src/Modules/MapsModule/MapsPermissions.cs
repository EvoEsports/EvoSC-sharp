using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.MapsModule;

/// <summary>
/// Exported so other modules can require these permissions without a reference to this module.
/// A permission group is still registered by the module that declares it, so the permission
/// set stays owned by MapsModule.
/// </summary>
[PermissionGroup]
[Export]
public enum MapsPermissions
{
    [Description("Allow adding maps to the server.")]
    AddMap,
    
    [Description("Allow removing maps from the server.")]
    RemoveMap
}
