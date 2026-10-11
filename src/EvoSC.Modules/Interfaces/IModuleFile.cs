namespace EvoSC.Modules.Interfaces;

public interface IModuleFile
{
    /// <summary>
    /// Information about this file.
    /// </summary>
    public FileInfo File { get; }

    /// <summary>
    /// Whether this file is an assembly (.dll) or not.
    /// </summary>
    public bool IsAssembly => File.Extension.Equals(".dll", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Verify the signature for this file.
    /// </summary>
    /// <returns></returns>
    public bool VerifySignature();

    /// <summary>
    /// Whether this file carries a valid signature from the key EvoSC uses to authorize a module
    /// for internal status without it being in the application's own known-module list. Separate
    /// from <see cref="VerifySignature"/>: that proves the file wasn't tampered with, this proves
    /// who published it.
    /// </summary>
    /// <returns></returns>
    public bool IsSignedByInternalModuleKey();
}
