using EvoSC.Manialinks.Interfaces.Themes;

namespace EvoSC.Manialinks.Themes;

public class ThemeInfo : IThemeInfo
{
    public required Type ThemeType { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required Guid ModuleId { get; init; }
}
