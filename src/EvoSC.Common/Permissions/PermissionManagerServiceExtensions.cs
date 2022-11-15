using EvoSC.Common.Interfaces.Services;
using SimpleInjector;

namespace EvoSC.Common.Permissions;

public static class PermissionManagerServiceExtensions
{
    public static Container AddEvoScPermissions(this Container container)
    {
        container.RegisterSingleton<IPermissionManager, PermissionManager>();
        return container;
    }
}
