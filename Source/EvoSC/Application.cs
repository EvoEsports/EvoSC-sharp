namespace EvoSC;

public class Application
{
    private readonly string[] _args;
    
    public Application(string[] args)
    {
        _args = args;
    }

    public Task RunAsync()
    {
        return Task.Delay(-1);
    }
}
