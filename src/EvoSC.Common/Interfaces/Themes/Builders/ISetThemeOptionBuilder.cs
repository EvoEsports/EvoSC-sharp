using EvoSC.Common.Themes;

namespace EvoSC.Common.Interfaces.Themes.Builders;

public interface ISetThemeOptionBuilder<TTheme> where TTheme : Theme<TTheme>
{
    public TTheme To(object value);
}
