using System.CommandLine;

namespace EvoSC.CLI.Interfaces;

public interface ICliContext
{
    public ParseResult ParseResult { get; }
    public int ExitCode { get; set; }
}
