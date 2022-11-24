using System.Text;

namespace EvoSC.Common.Util.TextFormatting;

public class TextFormatter
{
    private readonly List<FormattedText> _textParts = new();

    public TextFormatter AddText(StringBuilder text, TextStyling style)
    {
        _textParts.Add(new FormattedText(text, style));
        return this;
    }
    
    public TextFormatter AddText(string text, TextStyling style)
    {
        _textParts.Add(new FormattedText(text, style));
        return this;
    }
    
    public TextFormatter AddText(StringBuilder text)
    {
        _textParts.Add(new FormattedText(text));
        return this;
    }
    
    public TextFormatter AddText(string text)
    {
        _textParts.Add(new FormattedText(text));
        return this;
    }
    
    public TextFormatter AddText(StringBuilder text, Action<TextStyling> styleAction)
    {
        var style = new TextStyling();
        styleAction(style);
        _textParts.Add(new FormattedText(text, style));
        return this;
    }
    
    public TextFormatter AddText(string text, Action<TextStyling> styleAction)
    {
        var style = new TextStyling();
        styleAction(style);
        _textParts.Add(new FormattedText(text, style));
        return this;
    }

    public TextFormatter AddText(Action<FormattedText> textAction)
    {
        var formattedText = new FormattedText();
        textAction(formattedText);
        _textParts.Add(formattedText);
        return this;
    }

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
