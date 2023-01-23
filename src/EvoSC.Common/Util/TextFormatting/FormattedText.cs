using System.Text;

namespace EvoSC.Common.Util.TextFormatting;

/// <summary>
/// Class that represents a single text part that is formatted with a
/// specific style.
/// </summary>
public class FormattedText
{
    public TextStyling? Style { get; set; }
    public bool IsIsolated { get; set; }
    public StringBuilder Text { get; set; } = new StringBuilder();

    public FormattedText(){}

    /// <summary>
    /// Start formatted text from a specific string.
    /// </summary>
    /// <param name="text">The string to begin with.</param>
    public FormattedText(StringBuilder text)
    {
        Text = text;
    }

    /// <summary>
    /// Create formatted text from a string and a style.
    /// </summary>
    /// <param name="text">The string to start with.</param>
    /// <param name="style">The style to format the text with.</param>
    public FormattedText(StringBuilder text, TextStyling style) : this(text)
    {
        Style = style;
    }
    
    /// <summary>
    /// Start formatted text from a specific string.
    /// </summary>
    /// <param name="text">The string to begin with.</param>
    public FormattedText(string text) : this(new StringBuilder(text))
    {
    }
    
    /// <summary>
    /// Create formatted text from a string and a style.
    /// </summary>
    /// <param name="text">The string to start with.</param>
    /// <param name="style">The style to format the text with.</param>
    public FormattedText(string text, TextStyling style) : this(new StringBuilder(text), style)
    {
    }

    /// <summary>
    /// Set the text to be formatted.
    /// </summary>
    /// <param name="newText">Text to format.</param>
    /// <returns></returns>
    public FormattedText WithText(string newText)
    {
        Text.Clear();
        Text.Append(newText);
        return this;
    }

    /// <summary>
    /// Set the text to be formatted.
    /// </summary>
    /// <param name="newText">Text to format.</param>
    /// <returns></returns>
    public FormattedText WithText(StringBuilder newText)
    {
        // "duplicate" code to avoid the step of converting to string
        // and then back to string builder, for performance
        Text.Clear();
        Text.Append(newText);
        return this;
    }

    /// <summary>
    /// Append text.
    /// </summary>
    /// <param name="text">Text to append.</param>
    /// <returns></returns>
    public FormattedText AddText(string text)
    {
        Text.Append(text);
        return this;
    }
    
    /// <summary>
    /// Append text.
    /// </summary>
    /// <param name="text">Text to append.</param>
    /// <returns></returns>
    public FormattedText AddText(StringBuilder text)
    {
        Text.Append(text);
        return this;
    }

    /// <summary>
    /// Set the style to format the text with.
    /// </summary>
    /// <param name="style">The style to format the text with.</param>
    /// <returns></returns>
    public FormattedText WithStyle(TextStyling style)
    {
        Style = style;
        return this;
    }
    
    /// <summary>
    /// Set the style to format the text with.
    /// </summary>
    /// <param name="style">The style to format the text with.</param>
    /// <returns></returns>
    public FormattedText WithStyle(Action<TextStyling> styleAction)
    {
        var style = new TextStyling();
        styleAction(style);
        Style = style;
        return WithStyle(style);
    }

    /// <summary>
    /// Set this text's style as isolated so that it will not
    /// interfere with later text.
    /// </summary>
    /// <returns></returns>
    public FormattedText AsIsolated()
    {
        IsIsolated = true;
        return this;
    }

    /// <summary>
    /// Convert the current style and text to the formatted
    /// string output.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var formatted = new StringBuilder();

        if (Style != null)
        {
            formatted.Append(Style);
        }
        
        formatted.Append(Text);

        if (IsIsolated)
        {
            formatted.Insert(0, TextStyle.IsolationStart);
            formatted.Append(TextStyle.IsolationEnd);
        }
        
        return formatted.ToString();
    }
}
