using System.Drawing;
using System.Text;
using Microsoft.Extensions.Primitives;

namespace EvoSC.Common.Util.TextFormatting;

/// <summary>
/// Represents a style that a string can be formatted with.
/// </summary>
public class TextStyling
{
    private TextColor? _color { get; set; }
    private bool _isBold { get; set; }
    private bool _isWide { get; set; }
    private bool _isShadow { get; set; }
    private bool _isItalic { get; set; }
    private bool _isLink { get; set; }
    private string? _linkUrl { get; set; }
    private bool _isUppercase { get; set; }
    private bool _isNarrow { get; set; }
    private bool _isResetColor { get; set; }
    private bool _isStyleReset { get; set; }

    /// <summary>
    /// Set the color of this style.
    /// </summary>
    /// <param name="color">The color to use.</param>
    /// <returns></returns>
    public TextStyling WithColor(TextColor color)
    {
        _color = color;
        return this;
    }
    
    /// <summary>
    /// Set the color of this style.
    /// </summary>
    /// <param name="color">The color to use.</param>
    /// <returns></returns>
    public TextStyling WithColor(Color color)
    {
        _color = new TextColor(color);
        return this;
    }

    /// <summary>
    /// Set the text as bold.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsBold()
    {
        _isBold = true;
        return this;
    }

    /// <summary>
    /// Set the text as wide. This overrides narrow.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsWide()
    {
        _isWide = true;
        return this;
    }

    /// <summary>
    /// Set the text as narrow.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsNarrow()
    {
        _isNarrow = true;
        return this;
    }
    
    /// <summary>
    /// Add drop shadow to this style.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsDropShadow()
    {
        _isShadow = true;
        return this;
    }

    /// <summary>
    /// Set an italic style.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsItalic()
    {
        _isItalic = true;
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
        _isLink = true;
        _linkUrl = url;
        return this;
    }

    /// <summary>
    /// Set this style as a link so that the text after is the link itself.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsLink()
    {
        _isLink = true;
        return this;
    }

    /// <summary>
    /// Force uppercase of all characters.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsUppercase()
    {
        _isUppercase = true;
        return this;
    }

    /// <summary>
    /// Set this style as a color reset. Overrides color.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsColorReset()
    {
        _isResetColor = true;
        return this;
    }

    /// <summary>
    /// Set this style as a complete formatting reset. Overrides everything.
    /// </summary>
    /// <returns></returns>
    public TextStyling AsStyleReset()
    {
        _isStyleReset = true;
        return this;
    }
    
    /// <summary>
    /// Convert current style to a string representation.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var formatting = new StringBuilder();
        
        if (_isStyleReset)
        {
            return TextStyle.StyleReset;
        }

        if (_isResetColor)
        {
            formatting.Append(TextStyle.ColorReset);
        }
        else if (_color != null)
        {
            formatting.Append(_color);
        }

        if (_isBold)
        {
            formatting.Append(TextStyle.Bold);
        }
        
        if (_isWide)
        {
            formatting.Append(TextStyle.Wide);
        }
        else if (_isNarrow)
        {
            formatting.Append(TextStyle.Narrow);
        }

        if (_isShadow)
        {
            formatting.Append(TextStyle.DropShadow);
        }

        if (_isItalic)
        {
            formatting.Append(TextStyle.Italic);
        }

        if (_isUppercase)
        {
            formatting.Append(TextStyle.Uppercase);
        }

        if (!_isLink)
        {
            return formatting.ToString();
        }

        formatting.Append(TextStyle.Link);

        if (_linkUrl != null)
        {
            formatting.Append('[');
            formatting.Append(_linkUrl);
            formatting.Append(']');
        }

        return formatting.ToString();
    }
}
