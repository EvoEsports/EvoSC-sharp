using System.CommandLine;
using EvoSC.CLI.Interfaces;

namespace EvoSC.CLI.Models;

public class CliContext(ParseResult parseResult) : ICliContext
{
    public ParseResult ParseResult { get; } = parseResult;
    public int ExitCode { get; set; }
}
