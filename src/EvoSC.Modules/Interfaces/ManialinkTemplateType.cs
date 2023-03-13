using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Interfaces;

public enum ManialinkTemplateType
{
    [Identifier(Name = "mt", NoPrefix = true)]
    Template,
    
    [Identifier(Name = "ms", NoPrefix = true)]
    Script
}
