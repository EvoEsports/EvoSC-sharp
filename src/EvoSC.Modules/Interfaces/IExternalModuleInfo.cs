namespace EvoSC.Modules.Interfaces;

public interface IExternalModuleInfo : IModuleInfo
{
    public DirectoryInfo Directory { get; }
}
