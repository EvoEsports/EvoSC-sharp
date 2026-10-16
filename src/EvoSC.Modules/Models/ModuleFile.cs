using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Models;

public class ModuleFile(FileInfo file) : IModuleFile
{
    public FileInfo File { get; init; } = file;

    public bool VerifySignature()
    {
        // Signature verification is not implemented yet (see github #35); files are trusted as-is.
        return true;
    }
}
