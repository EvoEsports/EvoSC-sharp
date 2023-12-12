using EvoSC.Common.Themes;

namespace EvoSC.Common.Interfaces.Themes.Builders;

public interface IReplaceComponentBuilder<out TTheme>
where TTheme : Theme<TTheme>
{
    /// <summary>
    /// Replace a component with the provided component.
    /// </summary>
    /// <param name="newComponent">Name of the new component that will replace the old component.</param>
    /// <returns></returns>
    public TTheme With(string newComponent);
}
