using EvoSC.Manialinks.Themes;

namespace EvoSC.Manialinks.Interfaces.Themes.Builders;

public interface ISetThemeOptionBuilder<TTheme> where TTheme : Theme<TTheme>
{
    public TTheme To(object value);
}
