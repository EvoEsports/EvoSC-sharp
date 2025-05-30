using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Interfaces.Themes.Builders;
using EvoSC.Common.Themes.Builders;

namespace EvoSC.Common.Themes;

public abstract class Theme<TTheme> : ITheme, IThemeExpressions<TTheme> where TTheme : Theme<TTheme>
{
    public virtual Task ConfigureAsync(dynamic theme) => Task.CompletedTask;
    public virtual Task ConfigureDynamicAsync(dynamic theme) => Task.CompletedTask;

    public Dictionary<string, object> ThemeOptions { get; } = new();
    
    public Dictionary<string, string> ComponentReplacements { get; } = new();
    
    public ISetThemeOptionBuilder<TTheme> Set(string key) =>
        new SetThemeOptionBuilder<TTheme>(key, ThemeOptions, (TTheme)this);
    
    public IReplaceComponentBuilder<TTheme> Replace(string component) =>
        new ReplaceComponentBuilder<TTheme>(component, ComponentReplacements, (TTheme)this);
}
