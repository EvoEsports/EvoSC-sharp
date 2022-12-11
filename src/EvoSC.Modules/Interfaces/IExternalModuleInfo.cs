namespace EvoSC.Modules.Interfaces;

public interface IExternalModuleInfo : IModuleInfo
{
    bool IModuleInfo.IsInternal => false;

    public DirectoryInfo Directory { get; }
}
