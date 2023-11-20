using EvoSC.Common.Themes;

namespace EvoSC.Common.Interfaces.Themes.Builders;

public interface IReplaceComponentBuilder<out TTheme>
where TTheme : Theme<TTheme>
{
    public TTheme With(string newComponent);
}
