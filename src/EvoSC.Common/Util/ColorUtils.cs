using System.Globalization;
using ColorMine.ColorSpaces;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Util;

public class ColorUtils
{
    public string ToMlColor(string hexColor)
    {
        var color = new Hex(hexColor).ToRgb();
        
        var r = (color.R / 255.0).ToString("0.0000000", new NumberFormatInfo());
        var g = (color.G / 255.0).ToString("0.0000000", new NumberFormatInfo());
        var b = (color.B / 255.0).ToString("0.0000000", new NumberFormatInfo());

        return $"<{r}, {g}, {b}>";
    }
    
    /// <summary>
    /// Renders a color code in ManiaScript.
    /// </summary>
    /// <param name="hexColor">Color to render.</param>
    /// <returns></returns>
    public string ToMlColor(string hexColor)
    {
        var color = new Hex(hexColor).ToRgb();
        
        var r = (color.R / 255.0).ToString("0.0000000", new NumberFormatInfo());
        var g = (color.G / 255.0).ToString("0.0000000", new NumberFormatInfo());
        var b = (color.B / 255.0).ToString("0.0000000", new NumberFormatInfo());

        return $"<{r}, {g}, {b}>";
    }

    /// <summary>
    /// Set the opacity of a color using the alpha channel (4th byte).
    /// </summary>
    /// <param name="hexColor">Color to set opacity to</param>
    /// <param name="opacity">Opacity from 0-100</param>
    /// <returns></returns>
    public string Opacity(string hexColor, double opacity)
    {
        var color = new Hex(hexColor).ToRgb();

        var r = ((int)Math.Floor(color.R)).ToString("X2", CultureInfo.InvariantCulture);
        var g = ((int)Math.Floor(color.G)).ToString("X2", CultureInfo.InvariantCulture);
        var b = ((int)Math.Floor(color.B)).ToString("X2", CultureInfo.InvariantCulture);
        var a = ((int)Math.Floor(opacity / 100.0 * 255.0)).ToString("X2", CultureInfo.InvariantCulture);
        
        return $"{r}{g}{b}{a}";
    }
    
    /// <summary>
    /// Set the lightness for a color.
    /// </summary>
    /// <param name="hexColor">Hex color to set lightness for.</param>
    /// <param name="lightness">Lightness in percentage from 0-100.</param>
    /// <returns></returns>
    public static string SetLightness(string hexColor, float lightness)
    {
        var hsl = new Hex(hexColor).To<Hsl>();
        hsl.L = lightness / 100f;
        return hsl.To<Hex>().ToString().Substring(1);
    }

    /// <summary>
    /// Set the lightness for a color.
    /// </summary>
    /// <param name="color">Text color ot set lightness for.</param>
    /// <param name="lightness">Lightness in percentage from 0-100.</param>
    /// <returns></returns>
    public static string SetLightness(TextColor color, float lightness)
    {
        var rgb = new Rgb { R = color.R, G = color.G, B = color.B };
        var hsl = rgb.To<Hsl>();
        hsl.L = lightness / 100f;
        return hsl.To<Hex>().ToString().Substring(1);
    }
    
    /// <summary>
    /// Lighten a hex color by a set amount.
    /// </summary>
    /// <param name="hexColor">Hex color to lighten.</param>
    /// <param name="amount">Amount to increase the lightness to in percentage (0-100).</param>
    /// <returns></returns>
    public static string Lighten(string hexColor, float amount) =>
        AddLightness(
                new Hex(hexColor).To<Hsl>(),
                amount
            )
            .To<Hex>()
            .ToString()
            .Substring(1);

    /// <summary>
    /// Lighten a text color by a set amount.
    /// </summary>
    /// <param name="color">Text color to lighten.</param>
    /// <param name="amount">Amount to increase the lightness to in percentage (0-100).</param>
    /// <returns></returns>
    public static string Lighten(TextColor color, float amount) =>
        AddLightness(
                new Rgb { R = color.R, G = color.G, B = color.B }.To<Hsl>(),
                amount
            )
            .To<Hex>()
            .ToString()
            .Substring(1);

    /// <summary>
    /// Lighten a hex color by 10%.
    /// </summary>
    /// <param name="hexColor">Hex color to lighten.</param>
    /// <returns></returns>
    public static string Lighten(string hexColor) => Lighten(hexColor, 10);
    
    /// <summary>
    /// Lighten a text color by 10%.
    /// </summary>
    /// <param name="color">Text color to lighten.</param>
    /// <returns></returns>
    public static string Lighten(TextColor color) => Lighten(color, 10);

    /// <summary>
    /// Darken a hex color by a set amount.
    /// </summary>
    /// <param name="hexColor">Hex color to darken.</param>
    /// <param name="amount">Amount to darken in percentage (0-100).</param>
    /// <returns></returns>
    public static string Darken(string hexColor, float amount) => Lighten(hexColor, -amount);
   
   /// <summary>
    /// Darken a text color by a set amount.
    /// </summary>
    /// <param name="color">Text color to darken.</param>
    /// <param name="amount">Amount to darken in percentage (0-100).</param>
    /// <returns></returns>
    public static string Darken(TextColor color, float amount) => Lighten(color, -amount);
    
    /// <summary>
    /// Darken a hex color by 10%.
    /// </summary>
    /// <param name="hexColor">Hex color to darken.</param>
    /// <returns></returns>
    public static string Darken(string hexColor) => Darken(hexColor, 10);
    
    /// <summary>
    /// Darken a text color by 10%.
    /// </summary>
    /// <param name="color">Text color to darken.</param>
    /// <returns></returns>
    public static string Darken(TextColor color) => Darken(color, 10);

    /// <summary>
    /// Get the luma of an RGB color.
    /// </summary>
    /// <param name="color">Color to calculate luma from.</param>
    /// <remarks>The following method uses the BT. 709 coefficients to calculate the luma.</remarks>
    /// <returns></returns>
    public static double Luma(IRgb color) => (color.R * 0.2126 + color.G * 0.7152 + color.B * 0.0722)/255*100;
    
    /// <summary>
    /// Get the luma of a color.
    /// </summary>
    /// <param name="hexColor">Hex color to calculate luma from.</param>
    /// <returns></returns>
    public static double Luma(string hexColor) => Luma(new Hex(hexColor).ToRgb());
    
    /// <summary>
    /// Get the luma of a color.
    /// </summary>
    /// <param name="color">Text color to calculate luma from.</param>
    /// <returns></returns>
    public static double Luma(TextColor color) => Luma(new Rgb { R = color.R, G = color.G, B = color.B });

    /// <summary>
    /// Convert a hex color to it's grayscale representation.
    /// </summary>
    /// <param name="hexColor">Hex color to convert.</param>
    /// <returns></returns>
    public static string GrayScale(string hexColor)
    {
        var luma = Math.Round(Luma(hexColor)) / 100 * 255;
        return new Rgb { R = luma, G = luma, B = luma }
            .To<Hex>()
            .ToString()
            .Substring(1);
    }
    
    /// <summary>
    /// Convert a text color to it's grayscale representation.
    /// </summary>
    /// <param name="hexColor">text color to convert.</param>
    /// <returns></returns>
    public static string GrayScale(TextColor color)
    {
        var luma = Math.Round(Luma(color)) / 100 * 255;
        return new Rgb { R = luma, G = luma, B = luma }
            .To<Hex>()
            .ToString()
            .Substring(1);
    }
    
    private static Hsl AddLightness(Hsl hsl, float increase)
    {
        var newL = hsl.L + increase / 100f;

        newL = newL switch
        {
            > 1 => 1f,
            < 0 => 0,
            _ => newL
        };

        hsl.L = newL;

        return hsl;
    }
}
