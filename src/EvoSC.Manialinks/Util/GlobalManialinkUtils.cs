using EvoSC.Common.Interfaces.Themes;

namespace EvoSC.Manialinks.Util;

public class GlobalManialinkUtils
{
    private readonly dynamic _theme;

    public GlobalManialinkUtils(IThemeManager themeManager)
    {
        _theme = themeManager.Theme;
    }

    public string TypeToColorBg(string type) => type.ToLower() switch
    {
        "info" => _theme.Teal,
        "success" => _theme.Green,
        "warning" => _theme.Orange,
        "danger" => _theme.Red,
        "primary" => _theme.UI_BgPrimary,
        "secondary" => _theme.UI_BgSecondary,
        _ => (string)_theme.UI_BgPrimary
    };
}
