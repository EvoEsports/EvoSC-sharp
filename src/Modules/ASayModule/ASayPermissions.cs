using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.ASayModule;

[PermissionGroup]
public enum ASayPermissions
{
    [Description("Allows the usage of /asay and /clearasay commands.")]
    UseASay
}
