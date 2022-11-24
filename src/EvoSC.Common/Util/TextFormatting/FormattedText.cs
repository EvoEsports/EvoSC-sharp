using System.Text;
using Microsoft.Extensions.Primitives;

namespace EvoSC.Common.Util.TextFormatting;

public class FormattedText
{
    public TextStyling? Style { get; set; }
    public bool IsIsolated { get; set; }
    public StringBuilder Text { get; set; } = new StringBuilder();

    public FormattedText(){}

    public FormattedText(StringBuilder text)
    {
        Text = text;
    }

    public FormattedText(StringBuilder text, TextStyling style) : this(text)
    {
        Style = style;
    }
    
    public FormattedText(string text) : this(new StringBuilder(text))
    {
    }

    public FormattedText(string text, TextStyling style) : this(new StringBuilder(text), style)
    {
    }
    
    

    public FormattedText SetText(string newText)
    {
        Text.Clear();
        Text.Append(newText);
        return this;
    }

    public FormattedText SetText(StringBuilder newText)
    {
        // "duplicate" code to avoid the step of converting to string
        // and then back to string builder, for performance
        Text.Clear();
        Text.Append(newText);
        return this;
    }

    public FormattedText AddText(string text)
    {
        Text.Append(text);
        return this;
    }
    
    public FormattedText AddText(StringBuilder text)
    {
        Text.Append(text);
        return this;
    }

    public FormattedText WithStyle(TextStyling style)
    {
        Style = style;
        return this;
    }
    
    public FormattedText WithStyle(Action<TextStyling> styleAction)
    {
        var style = new TextStyling();
        styleAction(style);
        Style = style;
        return WithStyle(style);
    }

    public FormattedText AsIsolated()
    {
        IsIsolated = true;
        return this;
    }

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
