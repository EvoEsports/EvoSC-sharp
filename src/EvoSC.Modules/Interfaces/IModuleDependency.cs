namespace EvoSC.Modules.Interfaces;

public interface IModuleDependency
{
    public string Name { get; }
    public Version Version { get; }
}
