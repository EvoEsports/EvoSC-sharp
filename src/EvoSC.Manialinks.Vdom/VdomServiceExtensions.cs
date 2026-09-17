using EvoSC.Manialinks.Vdom.Patch;
using EvoSC.Manialinks.Vdom.Views;
using SimpleInjector;

namespace EvoSC.Manialinks.Vdom;

/// <summary>
/// Registers the reactive-view engine's singletons, matching
/// <c>EvoSC.Manialinks.ManialinkServiceExtensions</c>'s own pattern. Does not register
/// <see cref="VdomAckController"/> -- unlike a module's controllers, a plain library's
/// <c>[Controller]</c> classes are never auto-discovered (only <c>ModuleManager</c> scans for
/// those), so that happens as a separate startup action in <c>ApplicationSetup.cs</c>, after
/// <c>IControllerManager</c>'s action registries are wired up.
/// </summary>
public static class VdomServiceExtensions
{
    public static Container AddEvoScVdom(this Container services)
    {
        services.RegisterSingleton<IPatchTransport, LocalUserPatchTransport>();
        services.RegisterSingleton<IManialinkViewManager, ManialinkViewManager>();

        return services;
    }
}
