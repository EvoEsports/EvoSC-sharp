using EvoSC.Manialinks.Interfaces.Themes;

namespace EvoSC.Manialinks.Themes.Events.Args;

public class ThemeChangedEventArgs : EventArgs
{
    public required IThemeInfo ThemeInfo { get; init; }
    public required ITheme Theme { get; init; }
}
