using EvoSC.Manialinks.Themes;

namespace EvoSC.Manialinks.Interfaces.Themes.Builders;

public interface IReplaceComponentBuilder<out TTheme>
where TTheme : Theme<TTheme>
{
    public TTheme With(string newComponent);
}
