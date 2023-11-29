using EvoSC.Common.Interfaces.Themes;

namespace EvoSC.Manialinks.Util;

public class FontManialinkHelper
{
    private readonly dynamic _theme;

    public string Thin => ToThin(_theme.UI_Font);
    public string Regular => ToRegular(_theme.UI_Font);
    public string Bold => ToBold(_theme.UI_Font);
    public string ExtraBold => ToExtraBold(_theme.UI_Font);
    public string Mono => ToExtraBold(_theme.UI_Font);
    
    public FontManialinkHelper(IThemeManager theme)
    {
        _theme = theme.Theme;
    }
    
    public string ToRegular(string font) => font switch
    {
        _ when font.StartsWith("GameFont") => "GameFontSemiBold",
        _ when font.StartsWith("Rajdhani") => "RajdhaniMono",
        _ when font.StartsWith("Oswald") => "Oswald",
        _ when font.StartsWith("Roboto") => "RobotoCondensed",
        _ => throw new InvalidOperationException("Invalid font.")
    };

    public string ToThin(string font) => font switch
    {
        _ when font.StartsWith("GameFont") => "GameFontSemiRegular",
        _ => ToRegular(font)
    };
    
    public string ToBold(string font) => font switch
    {
        _ when font.StartsWith("GameFont") => "GameFontExtraBold",
        _ when font.StartsWith("Roboto") => "RobotoCondensedBold",
        _ => ToRegular(font)
    };
    
    public string ToExtraBold(string font) => font switch
    {
        _ when font.StartsWith("GameFont") => "GameFontBlack",
        _ => ToBold(font)
    };

    public string ToMono(string font) => font switch
    {
        _ when font.StartsWith("Rajdhani") => "RajdhaniMono",
        _ when font.StartsWith("Oswald") => "OswaldMono",
        _ => ToRegular(font)
    };
}
