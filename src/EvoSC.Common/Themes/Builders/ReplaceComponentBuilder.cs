using EvoSC.Common.Interfaces.Themes.Builders;

namespace EvoSC.Common.Themes.Builders;

public class ReplaceComponentBuilder<TTheme> : IReplaceComponentBuilder<TTheme>
where TTheme : Theme<TTheme>
{
    private readonly string _component;
    private readonly Dictionary<string, string> _componentReplacements;
    private readonly TTheme _theme;
    
    internal ReplaceComponentBuilder(string component, Dictionary<string, string> componentReplacements, TTheme theme)
    {
        _component = component;
        _componentReplacements = componentReplacements;
        _theme = theme;
    }
    
    public TTheme With(string newComponent)
    {
        _componentReplacements[_component] = newComponent;
        return _theme;
    }
}
