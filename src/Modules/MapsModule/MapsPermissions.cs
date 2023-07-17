using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.Maps;

[PermissionGroup]
public enum MapsPermissions
{
    [Description("Allow adding maps to the server.")]
    AddMap,
    
    [Description("Allow removing maps from the server.")]
    RemoveMap
}
