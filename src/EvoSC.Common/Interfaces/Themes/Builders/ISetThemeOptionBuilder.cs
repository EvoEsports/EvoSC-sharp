using EvoSC.Common.Themes;

namespace EvoSC.Common.Interfaces.Themes.Builders;

public interface ISetThemeOptionBuilder<TTheme> where TTheme : Theme<TTheme>
{
    /// <summary>
    /// Set a theme option value.
    /// </summary>
    /// <param name="value">Value of the theme option.</param>
    /// <returns></returns>
    public TTheme To(object value);
}
