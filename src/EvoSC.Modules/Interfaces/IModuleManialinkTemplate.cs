using EvoSC.Manialinks.Interfaces;

namespace EvoSC.Modules.Interfaces;

public interface IModuleManialinkTemplate
{
    public string Content { get; set; }
    public string Name { get; set; }
    public ManialinkTemplateType Type { get; set; }
}
