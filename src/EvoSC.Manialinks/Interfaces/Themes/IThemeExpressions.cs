using EvoSC.Manialinks.Interfaces.Themes.Builders;
using EvoSC.Manialinks.Themes;

namespace EvoSC.Manialinks.Interfaces.Themes;

public interface IThemeExpressions<TTheme>
where TTheme : Theme<TTheme>
{
    public ISetThemeOptionBuilder<TTheme> Set(string key);
    public IReplaceComponentBuilder<TTheme> Replace(string component);
}
