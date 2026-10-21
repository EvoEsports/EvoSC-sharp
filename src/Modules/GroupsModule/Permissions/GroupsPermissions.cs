using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.GroupsModule.Permissions;

[PermissionGroup]
public enum GroupsPermissions
{
    [Description("Allow creating, modifying and deleting groups and their permissions.")]
    ManageGroups,

    [Description("Allow adding and removing groups of players.")]
    ManagePlayerGroups
}
