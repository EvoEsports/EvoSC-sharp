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
}
