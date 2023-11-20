using EvoSC.Common.Interfaces.Themes.Builders;
using EvoSC.Common.Themes;

namespace EvoSC.Common.Interfaces.Themes;

public interface IThemeExpressions<TTheme>
where TTheme : Theme<TTheme>
{
    public ISetThemeOptionBuilder<TTheme> Set(string key);
    public IReplaceComponentBuilder<TTheme> Replace(string component);
}
