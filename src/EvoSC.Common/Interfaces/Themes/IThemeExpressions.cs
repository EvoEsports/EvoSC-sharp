using EvoSC.Common.Interfaces.Themes.Builders;
using EvoSC.Common.Themes;

namespace EvoSC.Common.Interfaces.Themes;

public interface IThemeExpressions<TTheme>
where TTheme : Theme<TTheme>
{
    /// <summary>
    /// Set a theme option.
    /// </summary>
    /// <param name="key">Name of the theme option.</param>
    /// <returns></returns>
    public ISetThemeOptionBuilder<TTheme> Set(string key);
    
    /// <summary>
    /// Replace a component.
    /// </summary>
    /// <param name="component">Name of the component to replace.</param>
    /// <returns></returns>
    public IReplaceComponentBuilder<TTheme> Replace(string component);
}
