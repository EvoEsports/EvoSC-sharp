using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Models;

public class ModuleFile(FileInfo file) : IModuleFile
{
    public FileInfo File { get; init; } = file;

    public bool VerifySignature()
    {
        // todo: github #35 https://github.com/EvoTM/EvoSC-sharp/issues/35
        return true;
    }
}
