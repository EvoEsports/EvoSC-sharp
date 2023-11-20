using EvoSC.Common.Themes;

namespace EvoSC.Common.Interfaces.Themes;

public interface IThemeManager
{
    public ITheme? SelectedTheme { get; }
    public IEnumerable<IThemeInfo> AvailableThemes { get; }
    public dynamic Theme { get; }
    
    public Task AddThemeAsync(Type themeType, Guid moduleId);
    public Task AddThemeAsync(Type themeType);
    public void RemoveTheme(string name);
    public void RemoveThemesForModule(Guid moduleId);
    public Task SetCurrentThemeAsync(string name);

    public dynamic GetCurrentThemeOptions();
}
