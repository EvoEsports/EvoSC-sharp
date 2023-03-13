using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Models;

public class ManiaScriptInfo : IManiaScriptInfo
{
    public required string Name { get; init; }
    public required string Content { get; init; }
}
