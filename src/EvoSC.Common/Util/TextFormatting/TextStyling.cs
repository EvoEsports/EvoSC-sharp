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

    /// <summary>
    /// Set the color of this style.
    /// </summary>
    /// <param name="color">The color to use.</param>
    /// <returns></returns>
    public TextStyling WithColor(TextColor color)
    {
        Color = color;
        return this;
    }
    
    /// <summary>
    /// Set the color of this style.
    /// </summary>
    /// <param name="color">The color to use.</param>
    /// <returns></returns>
    public TextStyling WithColor(Color color)
    {
        Color = new TextColor(color);
        return this;
    }

    /// <summary>
    /// Set the text as bold.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsBold()
    {
        IsBold = true;
        return this;
    }

    /// <summary>
    /// Set the text as wide. This overrides narrow.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsWide()
    {
        IsWide = true;
        return this;
    }

    /// <summary>
    /// Set the text as narrow.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsNarrow()
    {
        IsNarrow = true;
        return this;
    }
    
    /// <summary>
    /// Add drop shadow to this style.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsDropShadow()
    {
        IsShadow = true;
        return this;
    }

    /// <summary>
    /// Set an italic style.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsItalic()
    {
        IsItalic = true;
        return this;
    }

    /// <summary>
    /// Add a link to this style. The text after will be a clickable link
    /// to the specified url.
    /// </summary>
    /// <param name="url">The url to link to.</param>
    /// <returns></returns>
    public TextStyling WithLink(string url)
    {
        IsLink = true;
        LinkUrl = url;
        return this;
    }

    /// <summary>
    /// Set this style as a link so that the text after is the link itself.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsLink()
    {
        IsLink = true;
        return this;
    }

    /// <summary>
    /// Force uppercase of all characters.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsUppercase()
    {
        IsUppercase = true;
        return this;
    }

    /// <summary>
    /// Set this style as a color reset. Overrides color.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsColorReset()
    {
        IsResetColor = true;
        return this;
    }

    /// <summary>
    /// Set this style as a complete formatting reset. Overrides everything.
    /// </summary>
    /// <returns></returns>
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
