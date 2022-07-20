using EvoSC.Core.Plugins;
using ExamplePlugin2;
using Microsoft.Extensions.DependencyInjection;

namespace ExamplePlugin;

public class ExamplePlugin : EvoSCPlugin
{   
    public static void Setup(IServiceCollection services)
    {
        Console.WriteLine("hello example plugin");
        var test = new TestClass();
        test.Hello();
    }
}