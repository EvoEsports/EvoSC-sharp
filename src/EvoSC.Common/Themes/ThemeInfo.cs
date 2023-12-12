using EvoSC.Common.Interfaces.Themes;

namespace EvoSC.Common.Themes;

public class ThemeInfo : IThemeInfo
{
    public required Type ThemeType { get; init; }
    
    public required string Name { get; init; }
    
    public required string Description { get; init; }
    
    public required Type? OverrideTheme { get; init; }
    
    public required Guid ModuleId { get; init; }
    
    public Type EffectiveThemeType => OverrideTheme ?? ThemeType;
}
