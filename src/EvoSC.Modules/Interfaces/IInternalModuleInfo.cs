namespace EvoSC.Modules.Interfaces;

public interface IInternalModuleInfo : IModuleInfo
{
    bool IModuleInfo.IsInternal => true;
}
