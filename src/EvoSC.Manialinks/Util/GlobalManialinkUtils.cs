using System.Globalization;
using System.Text;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Util;

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
        "primary" => _theme.UI_AccentPrimary,
        "secondary" => _theme.UI_AccentSecondary,
        _ => _theme.UI_BgPrimary
    };

    public string TypeToColorText(string type)
    {
        var bgColor = TypeToColorBg(type);
        var luma = ColorUtils.Luma(bgColor);

        return luma <= 50 ? ColorUtils.Lighten(bgColor, 40) : ColorUtils.Darken(bgColor, 40);
    } 

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

    public double ColorOpacity(string color)
    {
        if (color.Length != 8)
        {
            return 1;
        }

        var opacity = Convert.FromHexString(color[6..]).First();

        return Math.Round(opacity / 255.0, 2);

    }

    public int[] Range(int n) => Enumerable.Range(0, n).ToArray();

    public bool HasItem(string items, string item, string splitter)
    {
        var itemsSplit = items.Split(splitter);
        return itemsSplit.Contains(item);
    }

    public bool HasItem(string items, string item) => HasItem(items, item, ",");
}
