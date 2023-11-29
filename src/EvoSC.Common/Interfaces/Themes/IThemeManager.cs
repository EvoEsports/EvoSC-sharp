using EvoSC.Common.Themes;

namespace EvoSC.Common.Interfaces.Themes;

public interface IThemeManager
{
    public IEnumerable<IThemeInfo> AvailableThemes { get; }
    public dynamic Theme { get; }
    public Dictionary<string, string> ComponentReplacements { get; }
    
    public Task AddThemeAsync(Type themeType, Guid moduleId);
    public Task AddThemeAsync(Type themeType);
    public Task RemoveTheme(string name);
    public Task RemoveThemesForModule(Guid moduleId);
    public Task<ITheme> ActivateThemeAsync(string name);
    public void InvalidateCache();
}