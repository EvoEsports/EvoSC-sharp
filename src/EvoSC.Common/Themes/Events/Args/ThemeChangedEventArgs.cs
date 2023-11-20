using EvoSC.Common.Interfaces.Themes;

namespace EvoSC.Common.Themes.Events.Args;

public class ThemeChangedEventArgs : EventArgs
{
    public required IThemeInfo ThemeInfo { get; init; }
    public required ITheme Theme { get; init; }
}
