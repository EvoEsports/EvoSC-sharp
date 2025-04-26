using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace EvoSC.Common.Services;

public static class CommonServiceExtensions
{
    public static Container AddEvoScCommonScopedServices(this Container services)
    {
        services.Register<IContextService, ContextService>(Lifestyle.Scoped);
        
        return services;
    }

    public static void ConfigureServiceContainerForEvoSc(this Container services)
    {
        services.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
        services.Options.EnableAutoVerification = false;
        services.Options.ResolveUnregisteredConcreteTypes = true;
        services.Options.SuppressLifestyleMismatchVerification = true;
        services.Options.UseStrictLifestyleMismatchBehavior = false;
    }
}
