namespace EvoSC.Common.Interfaces.Application;

public interface IStartupComponent
{
    public string Name { get; }
    public List<string> Dependencies { get; }
}
