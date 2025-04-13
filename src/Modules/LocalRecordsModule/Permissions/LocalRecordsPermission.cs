using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.LocalRecordsModule.Permissions;

[PermissionGroup]
public enum LocalRecordsPermission
{
    [Description("Can reset the local records table on all maps.")]
    ResetLocals,
    
    [Description("Can transfer the local records between maps.")]
    TransferLocals,
}
