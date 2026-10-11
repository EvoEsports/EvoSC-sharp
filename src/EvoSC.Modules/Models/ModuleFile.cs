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

    public bool IsSignedByInternalModuleKey()
    {
        // Not implemented yet, same as VerifySignature (see github #35). Fails closed, unlike
        // VerifySignature above: until this check is real, a module cannot gain internal status
        // through signing alone, only by being in the application's own known-module list.
        return false;
    }
}
