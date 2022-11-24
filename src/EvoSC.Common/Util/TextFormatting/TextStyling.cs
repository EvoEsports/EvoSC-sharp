using System.Drawing;
using System.Text;
using Microsoft.Extensions.Primitives;

namespace EvoSC.Common.Util.TextFormatting;

public class TextStyling
{
    public TextColor? Color { get; set; }
    public bool IsBold { get; set; }
    public bool IsWide { get; set; }
    public bool IsShadow { get; set; }
    public bool IsItalic { get; set; }
    public bool IsLink { get; set; }
    public string? LinkUrl { get; set; }
    public bool IsUppercase { get; set; }
    public bool IsNarrow { get; set; }
    public bool IsResetColor { get; set; }
    public bool IsStyleReset { get; set; }

    public TextStyling SetColor(TextColor color)
    {
        Color = color;
        return this;
    }
    
    public TextStyling SetColor(Color color)
    {
        Color = new TextColor(color);
        return this;
    }

    public TextStyling SetBold()
    {
        IsBold = true;
        return this;
    }

    public TextStyling SetWide()
    {
        IsWide = true;
        return this;
    }

    public TextStyling SetNarrow()
    {
        IsNarrow = true;
        return this;
    }
    
    public TextStyling SetDropShadow()
    {
        IsShadow = true;
        return this;
    }

    public TextStyling SetItalic()
    {
        IsItalic = true;
        return this;
    }

    public TextStyling SetLink(string url)
    {
        IsLink = true;
        LinkUrl = url;
        return this;
    }

    public TextStyling SetUppercase()
    {
        IsUppercase = true;
        return this;
    }

    public TextStyling AsColorReset()
    {
        IsResetColor = true;
        return this;
    }

    public TextStyling AsStyleReset()
    {
        IsStyleReset = true;
        return this;
    }
    
    /// <summary>
    /// Convert current style to a string representation.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var formatting = new StringBuilder();
        
        if (IsStyleReset)
        {
            return TextStyle.StyleReset;
        }

        if (IsResetColor)
        {
            formatting.Append(TextStyle.ColorReset);
        }
        else if (this.Color != null)
        {
            formatting.Append(this.Color);
        }

        if (IsBold)
        {
            formatting.Append(TextStyle.Bold);
        }
        
        if (IsWide)
        {
            formatting.Append(TextStyle.Wide);
        }
        else if (IsNarrow)
        {
            formatting.Append(TextStyle.Narrow);
        }

        if (IsShadow)
        {
            formatting.Append(TextStyle.DropShadow);
        }

        if (IsItalic)
        {
            formatting.Append(TextStyle.Italic);
        }

        if (IsUppercase)
        {
            formatting.Append(TextStyle.Uppercase);
        }

        if (!IsLink)
        {
            return formatting.ToString();
        }

        formatting.Append(TextStyle.Link);

        if (LinkUrl != null)
        {
            formatting.Append('[');
            formatting.Append(LinkUrl);
            formatting.Append(']');
        }

        return formatting.ToString();
    }
}
