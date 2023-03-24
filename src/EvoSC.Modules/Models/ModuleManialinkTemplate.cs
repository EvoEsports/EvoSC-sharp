using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Models;

public class ModuleManialinkTemplate : IModuleManialinkTemplate
{
    public required string Content { get; set; }
    public required string Name { get; set; }
    public required ManialinkTemplateType Type { get; set; }
}
