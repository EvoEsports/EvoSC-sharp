using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Interfaces.Themes.Builders;
using EvoSC.Common.Themes.Builders;

namespace EvoSC.Common.Themes;

public abstract class Theme<TTheme> : IDefaultThemeOptions, ITheme, IThemeExpressions<TTheme> where TTheme : Theme<TTheme>
{
    public abstract Task ConfigureAsync();

    public Dictionary<string, object> ThemeOptions { get; } = new();

    public Dictionary<string, string> ComponentReplacements { get; } = new();

    public ISetThemeOptionBuilder<TTheme> Set(string key) =>
        new SetThemeOptionBuilder<TTheme>(key, ThemeOptions, (TTheme)this);

    public IReplaceComponentBuilder<TTheme> Replace(string component) =>
        new ReplaceComponentBuilder<TTheme>(component, ComponentReplacements, (TTheme)this);
}
