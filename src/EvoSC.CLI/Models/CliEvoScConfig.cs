using System.CommandLine.Binding;

namespace EvoSC.CLI.Models;

public class CliEvoScConfig
{
    public Dictionary<string, object> Options { get; set; }
}
