using EvoSC.Core.Commands.Chat;
using EvoSC.Core.Commands.Generic.Attributes;
using EvoSC.Core.Services;
using ExamplePlugin2;

namespace ExamplePlugin;

public class Commands : ChatCommandGroup
{
    private readonly ExampleService _example;
    
    public Commands(ExampleService example)
    {
        _example = example;
    }
    
    [Command("example", "Just an example command")]
    public Task Example()
    {
        _example.DoSomething();
        return Task.CompletedTask;
    }
}