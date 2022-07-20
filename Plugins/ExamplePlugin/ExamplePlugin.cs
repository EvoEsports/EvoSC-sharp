using EvoSC.Core.Plugins;
using ExamplePlugin2;

namespace ExamplePlugin;

public class ExamplePlugin : EvoSCPlugin
{   
    public static void Setup()
    {
        Console.WriteLine("hello example plugin");
        var test = new TestClass();
        test.Hello();
    }
}