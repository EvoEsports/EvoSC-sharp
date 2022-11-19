using EvoSC.Common.Interfaces.Services;
using SimpleInjector;

namespace EvoSC.Common.Permissions;

public static class PermissionManagerServiceExtensions
{
    /// <summary>
    /// Add the permission system to the application's service container.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public static Container AddEvoScPermissions(this Container container)
    {
        container.RegisterSingleton<IPermissionManager, PermissionManager>();
        return container;
    }
}
