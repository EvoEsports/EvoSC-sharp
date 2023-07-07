namespace EvoSC.CLI.Models;

public class CliEvoScConfig
{
    /// <summary>
    /// Key/Value pairs presenting a config value set through the CLI.
    /// </summary>
    public Dictionary<string, object> Options { get; set; }
}
