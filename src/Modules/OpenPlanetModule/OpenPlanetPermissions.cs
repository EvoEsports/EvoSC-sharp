using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.OpenPlanetModule;

[PermissionGroup]
public enum OpenPlanetPermissions
{
    [Description("Allows players to bypass OpenPlanet signature mode verification.")]
    CanBypassVerification
}
