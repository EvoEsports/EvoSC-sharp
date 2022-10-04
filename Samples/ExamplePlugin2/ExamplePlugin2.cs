using EvoSC.Core.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace ExamplePlugin2;

public class ExamplePlugin2 : EvoSCPlugin
{
    public static void Setup(IServiceCollection services)
    {
        services.AddTransient<TestClass>();
    }
}