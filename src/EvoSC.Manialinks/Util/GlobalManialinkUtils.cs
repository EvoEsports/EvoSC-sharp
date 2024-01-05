using System.Globalization;
using EvoSC.Common.Interfaces.Themes;

namespace EvoSC.Manialinks.Util;

public class GlobalManialinkUtils(IThemeManager themeManager)
{
    private readonly dynamic _theme = themeManager.Theme;
    private readonly GameIcons _icons = new();

    /// <summary>
    /// Status type to a color.
    /// </summary>
    /// <param name="type">Name of the status.</param>
    /// <returns></returns>
    public string TypeToColorBg(string type) => type.ToLower(CultureInfo.InvariantCulture) switch
    {
        "info" => _theme.Teal,
        "success" => _theme.Green,
        "warning" => _theme.Orange,
        "danger" => _theme.Red,
        "primary" => _theme.UI_BgPrimary,
        "secondary" => _theme.UI_BgSecondary,
        _ => _theme.UI_BgPrimary
    };

    /// <summary>
    /// Status type to an icon.
    /// </summary>
    /// <param name="type">Name of the status.</param>
    /// <returns></returns>
    public string TypeToIcon(string type) => type.ToLower(CultureInfo.InvariantCulture) switch
    {
        "info" => _icons.InfoCircle,
        "success" => _icons.CheckCircle,
        "warning" => _icons.ExclamationCircle,
        "danger" => _icons.TimesCircle,
        "primary" => _icons.ExclamationCircle,
        "secondary" => _icons.ExclamationCircle,
        _ => _icons.ExclamationCircle
    };

    public string RandomId(string id) => $"{id}_{new Guid().ToString()}";

    public string DefaultOrRandomId(string defaultId, string id) =>
        id == defaultId ? $"{id}_{new Guid().ToString()}" : id;
}
