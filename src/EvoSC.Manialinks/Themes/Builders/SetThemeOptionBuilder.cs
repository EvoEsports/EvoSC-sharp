using EvoSC.Manialinks.Interfaces.Themes.Builders;

namespace EvoSC.Manialinks.Themes.Builders;

public class SetThemeOptionBuilder<TTheme> : ISetThemeOptionBuilder<TTheme>
where TTheme : Theme<TTheme>
{
    private readonly string _key;
    private readonly Dictionary<string, object> _themeOptions;
    private readonly TTheme _theme;
    
    internal SetThemeOptionBuilder(string key, Dictionary<string, object> themeOptions, TTheme theme)
    {
        _key = key;
        _themeOptions = themeOptions;
        _theme = theme;
    }
    
    public TTheme To(object value)
    {
        _themeOptions[_key] = value;
        return _theme;
    }
}
