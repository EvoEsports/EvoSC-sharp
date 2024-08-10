using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.Manialinks;

public enum WidgetPosition
{
    [Identifier(Name = "left", NoPrefix = true)]
    Left,

    [Identifier(Name = "right", NoPrefix = true)]
    Right,

    [Identifier(Name = "center", NoPrefix = true)]
    Center,

    [Identifier(Name = "", NoPrefix = true)]
    Undefined,
}
