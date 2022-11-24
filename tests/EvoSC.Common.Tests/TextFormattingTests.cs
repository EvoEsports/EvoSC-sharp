using System.Drawing;
using EvoSC.Common.Util;
using EvoSC.Common.Util.TextFormatting;
using Xunit;

namespace EvoSC.Common.Tests;

public class TextFormattingTests
{
    [Fact]
    public void Style_Output_Formatted_Red_Color()
    {
        var style = new TextStyling()
            .WithColor(Color.Red);

        var result = style.ToString();

        Assert.Equal("$f00", result);
    }

    [Theory]
    [InlineData("SetBold", "$o")]
    [InlineData("SetWide", "$w")]
    [InlineData("SetNarrow", "$n")]
    [InlineData("SetDropShadow", "$s")]
    [InlineData("SetItalic", "$i")]
    [InlineData("SetUppercase", "$t")]
    public void Style_Outputs_Correct_Formatted_Style(string method, string expected)
    {
        var style = new TextStyling();

        ReflectionUtils.CallMethod(typeof(TextStyling), style, method);

        var result = style.ToString();
        
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Style_Overrides_Color_When_Reset()
    {
        var style = new TextStyling()
            .WithColor(Color.Red)
            .AsColorReset();

        var result = style.ToString();
        
        Assert.Equal("$g", result);
    }

    [Fact]
    public void Style_Overrides_All_Styles_When_Reset()
    {
        var style = new TextStyling()
            .WithColor(Color.Red)
            .AsBold()
            .AsWide()
            .AsItalic()
            .AsLink()
            .WithLink("test")
            .AsUppercase()
            .AsNarrow()
            .AsDropShadow()
            .AsStyleReset();

        var result = style.ToString();
        
        Assert.Equal("$z", result);
    }

    [Fact]
    public void Style_Narrow_And_Wide_Dont_Conflict()
    {
        var style = new TextStyling()
            .AsWide()
            .AsNarrow();

        var result = style.ToString();
        
        Assert.Equal("$w", result);
    }
}
