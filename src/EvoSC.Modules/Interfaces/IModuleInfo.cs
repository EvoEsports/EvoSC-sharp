namespace EvoSC.Modules.Interfaces;

public interface IModuleInfo
{
    public string Name { get; }
    public string Title { get; }
    public string Summary { get; }
    public Version Version { get; }
    public string Author { get; }
    public IEnumerable<IModuleInfo> Dependencies { get; }
    public bool IsInternal { get; }
}
