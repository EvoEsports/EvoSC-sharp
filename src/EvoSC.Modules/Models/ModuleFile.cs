using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Models;

public class ModuleFile : IModuleFile
{
    public FileInfo File { get; init; }

    public ModuleFile(FileInfo file)
    {
        File = file;
    }
    
    public bool VerifySignature()
    {
        // todo: github #35 https://github.com/EvoTM/EvoSC-sharp/issues/35
        return true;
    }
}
