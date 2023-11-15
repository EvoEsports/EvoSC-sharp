namespace EvoSC.Manialinks.Interfaces.Themes;

public interface IThemeInfo
{
    public Type ThemeType { get; }
    public string Name { get; }
    public string Description { get; }
    
    public Guid ModuleId { get; }
}
