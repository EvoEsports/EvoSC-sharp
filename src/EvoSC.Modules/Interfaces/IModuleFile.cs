namespace EvoSC.Modules.Interfaces;

public interface IModuleFile
{
    public FileInfo File { get; }

    public bool IsAssembly => File.Extension.Equals(".dll", StringComparison.OrdinalIgnoreCase);

    public bool VerifySignature();
}
