using EvoSC.Core.Plugins;

namespace ExamplePlugin;

public class ExamplePlugin : EvoSCPlugin
{   
    public static void Setup()
    {
        Console.WriteLine("hello example plugin");
    }
}