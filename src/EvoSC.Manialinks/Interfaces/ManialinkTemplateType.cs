using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Manialinks.Interfaces;

/// <summary>
/// Possible template types for Manialink templates.
/// </summary>
public enum ManialinkTemplateType
{
    /// <summary>
    /// XML manialink template.
    /// </summary>
    [Identifier(Name = "mt", NoPrefix = true)]
    Template,
    
    /// <summary>
    /// ManiaScript code.
    /// </summary>
    [Identifier(Name = "ms", NoPrefix = true)]
    Script
}
