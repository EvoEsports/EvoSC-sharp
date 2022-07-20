using EvoSC.Core.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace ExamplePlugin;

public class ExamplePlugin : EvoSCPlugin
{   
    public static void Setup(IServiceCollection services)
    {
        Console.WriteLine("hello example plugin");
    }
}