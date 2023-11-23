namespace EvoSC.Common.Interfaces.Themes;

public interface IThemeInfo
{
    public Type ThemeType { get; }
    public string Name { get; }
    public string Description { get; }
    
    public Type? OverrideTheme { get; }
    
    public Guid ModuleId { get; }
    
    public Type EffectiveThemeType { get; }
}
