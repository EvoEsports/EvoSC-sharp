using System.Text;

namespace EvoSC.Common.Util.TextFormatting;

/// <summary>
/// Represents a multi-formatted string.
/// </summary>
public class TextFormatter
{
    private readonly List<FormattedText> _textParts = new();

    /// <summary>
    /// Add a styling component to the text.
    /// </summary>
    /// <param name="style">The style to add.</param>
    /// <returns></returns>
    public TextFormatter AddStyle(TextStyling style)
    {
        _textParts.Add(new FormattedText("", style));
        return this;
    }
    
    /// <summary>
    /// Add a styling component to the text using a builder action.
    /// </summary>
    /// <param name="styleBuilder">The- style to add.</param>
    /// <returns></returns>
    public TextFormatter AddStyle(Action<TextStyling> styleBuilder)
    {
        var style = new TextStyling();
        styleBuilder(style);
        _textParts.Add(new FormattedText("", style));
        return this;
    }
    
    /// <summary>
    /// Add text and styling.
    /// </summary>
    /// <param name="text">Text to add.</param>
    /// <param name="style">Style for this text.</param>
    /// <returns></returns>
    public TextFormatter AddText(StringBuilder text, TextStyling style)
    {
        _textParts.Add(new FormattedText(text, style));
        return this;
    }
    
    /// <summary>
    /// Add text and styling.
    /// </summary>
    /// <param name="text">Text to add.</param>
    /// <param name="style">Style for this text.</param>
    /// <returns></returns>
    public TextFormatter AddText(string text, TextStyling style)
    {
        _textParts.Add(new FormattedText(text, style));
        return this;
    }
    
    /// <summary>
    /// Add pure text with default formatting.
    /// </summary>
    /// <param name="text">Text to add.</param>
    /// <returns></returns>
    public TextFormatter AddText(StringBuilder text)
    {
        _textParts.Add(new FormattedText(text));
        return this;
    }
    
    /// <summary>
    /// Add text with default formatting.
    /// </summary>
    /// <param name="text">Text to add.</param>
    /// <returns></returns>
    public TextFormatter AddText(string text)
    {
        _textParts.Add(new FormattedText(text));
        return this;
    }
    
    /// <summary>
    /// Add text and style it with a builder.
    /// </summary>
    /// <param name="text">Text to add.</param>
    /// <param name="styleAction">Action lambda to configure the style.</param>
    /// <returns></returns>
    public TextFormatter AddText(StringBuilder text, Action<TextStyling> styleAction)
    {
        var style = new TextStyling();
        styleAction(style);
        _textParts.Add(new FormattedText(text, style));
        return this;
    }
    
    /// <summary>
    /// Add text and style it with a builder.
    /// </summary>
    /// <param name="text">Text to add.</param>
    /// <param name="styleAction">Action lambda to configure the style.</param>
    /// <returns></returns>
    public TextFormatter AddText(string text, Action<TextStyling> styleAction)
    {
        var style = new TextStyling();
        styleAction(style);
        _textParts.Add(new FormattedText(text, style));
        return this;
    }

    /// <summary>
    /// Add formatted text using a builder action.
    /// </summary>
    /// <param name="textAction">The formatted text to add.</param>
    /// <returns></returns>
    public TextFormatter AddText(Action<FormattedText> textAction)
    {
        var formattedText = new FormattedText();
        textAction(formattedText);
        _textParts.Add(formattedText);
        return this;
    }

    /// <summary>
    /// Convert the complete text into the raw formatted representation.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var formatted = new StringBuilder();

        foreach (var part in _textParts)
        {
            formatted.Append(part);
        }
        
        return formatted.ToString();
    }
}
