using System.Diagnostics;
using System.Drawing;

namespace EvoSC.Common.Util.TextFormatting;

public class TextColor
{
    private readonly byte _r;
    private readonly byte _g;
    private readonly byte _b;
    
    public TextColor(Color color)
    {
        _r = (byte)Math.Round((float)color.R / 0xff * 0xf);
        _g = (byte)Math.Round((float)color.G / 0xff * 0xf);
        _b = (byte)Math.Round((float)color.B / 0xff * 0xf);
    }

    public TextColor(byte r, byte g, byte b)
    {
        if (r is < 0 or > 0xf || g is < 0 or > 0xf || b is < 0 or > 0xf)
        {
            throw new InvalidCastException("Invalid RGB colors, must be between 0 and 15.");
        }
        
        _r = r;
        _g = g;
        _b = b;
    }

    private static string ToHex(byte n) => n switch
    {
        >= 0 and <= 9 => n.ToString(),
        10 => "a",
        11 => "b",
        12 => "c",
        13 => "d",
        14 => "e",
        15 => "f",
        _ => ""
    };
    
    /// <summary>
    /// Convert this color to the formatted text representation.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"${ToHex(_r)}{ToHex(_g)}{ToHex(_b)}";
    }
}
