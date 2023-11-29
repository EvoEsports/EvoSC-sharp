﻿using EvoSC.Common.Interfaces.Themes;

namespace EvoSC.Manialinks.Util;

public class GlobalManialinkUtils
{
    private readonly dynamic _theme;
    private readonly GameIcons _icons = new GameIcons();

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
        _ => _theme.UI_BgPrimary
    };

    public string TypeToIcon(string type) => type.ToLower() switch
    {
        "info" => _icons.InfoCircle,
        "success" => _icons.CheckCircle,
        "warning" => _icons.ExclamationCircle,
        "danger" => _icons.TimesCircle,
        "primary" => _icons.ExclamationCircle,
        "secondary" => _icons.ExclamationCircle,
        _ => _icons.ExclamationCircle
    };
}