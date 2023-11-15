namespace EvoSC.Manialinks.Interfaces.Themes;

public interface IThemeManager
{
    public ITheme? CurrentTheme { get; }
    public IEnumerable<IThemeInfo> AvailableThemes { get; }
    
    public Task AddThemeAsync(Type themeType, Guid moduleId);
    public Task AddThemeAsync(Type themeType);
    public void RemoveTheme(string name);
    public Task SetCurrentThemeAsync(string name);
}
