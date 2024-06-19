using System.Globalization;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Util;
using EvoSC.Common.Util.TextFormatting;

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
    public string TypeToColorBg(string type)
    {
        var color = type.ToLower(CultureInfo.InvariantCulture) switch
        {
            "info" => _theme.Teal,
            "success" => _theme.Green,
            "warning" => _theme.Orange,
            "danger" => _theme.Red,
            "primary" => _theme.UI_SurfaceBgPrimary,
            "secondary" => _theme.UI_SurfaceBgSecondary,
            "accent" => _theme.UI_AccentPrimary,
            _ => _theme.UI_BgPrimary
        };

        return color.ToString();
    }

    public string TypeToColorText(string type)
    {
        var bgColor = TypeToColorBg(type);
        var luma = ColorUtils.Luma(bgColor);
        
        return luma <= 50 ? _theme.UI_TextPrimary : _theme.UI_TextSecondary;
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

    public string RandomId(string id) => $"{id}_{Guid.NewGuid().ToString()}";

    public string DefaultOrRandomId(string defaultId, string id) =>
        id == defaultId ? $"{id}_{Guid.NewGuid().ToString()}" : id;

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

    public string StyledTime(IRaceTime time)
    {
        var styledTime = new TextFormatter();

        if (time.Hours > 0)
        {
            styledTime.AddText(time.Hours + ":", s => s.WithColor(_theme.UI_TextPrimary));
        }

        styledTime.AddText(time.Minutes.ToString().PadLeft(2, '0'), s => s
            .WithColor(time.Minutes + time.Hours > 0 ? _theme.UI_TextPrimary : _theme.UI_TextMuted)
        );

        styledTime.AddText(":", s => s.WithColor(_theme.UI_TextPrimary));
        styledTime.AddText(time.Seconds.ToString().PadLeft(2, '0'), s => s
            .WithColor(time.Seconds + time.Minutes > 0 ? _theme.UI_TextPrimary : _theme.UI_TextMuted)
        );
        
        styledTime.AddText(".", s => s.WithColor(_theme.UI_TextPrimary));
        styledTime.AddText(time.Milliseconds.ToString().PadLeft(3, '0'), s => s
            .WithColor(time.Milliseconds + time.Seconds > 0 ? _theme.UI_TextPrimary : _theme.UI_TextMuted)
        );

        Console.WriteLine(styledTime.ToString());
        return styledTime.ToString();
    }
}
