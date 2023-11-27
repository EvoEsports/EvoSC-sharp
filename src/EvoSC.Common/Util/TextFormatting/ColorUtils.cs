using ColorMine.ColorSpaces;

namespace EvoSC.Common.Util.TextFormatting;

public static class ColorUtils
{
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
    /// Lighten a texdt color by 10%.
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
