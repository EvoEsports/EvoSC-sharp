using System;
using EvoSC.Common.Config.Mapping;
using EvoSC.Common.Util.TextFormatting;
using Xunit;

namespace EvoSC.Common.Tests.Config.Mapping;

public class TextColorTypeParserTests
{
    [Theory]
    [InlineData("123", true, new byte[]{1,2,3})]
    [InlineData("000", true, new byte[]{0,0,0})]
    [InlineData("fff", true, new byte[]{0xf,0xf,0xf})]
    [InlineData("", false, new byte[]{})]
    [InlineData("00x", false, new byte[]{})]
    [InlineData("abcaaaaaaaa", false, new byte[]{})]
    [InlineData("1ab", true, new byte[]{0x1, 0xa, 0xb})]
    public void Text_Color_Parsed(string value, bool shouldBeParsed, byte[] expected)
    {
        var colorParser = new TextColorTypeParser();

        bool parsed = colorParser.TryParse(value, null, out var result);

        if (shouldBeParsed)
        {
            Assert.NotNull(result);
            Assert.True(parsed);
            Assert.Equal(new TextColor(expected[0], expected[1], expected[2]).ToString(), result.ToString());
        }
        else
        {
            Assert.False(parsed);
        }
    }

    [Fact]
    public void Supported_Types_Is_TextColor()
    {
        var colorParser = new TextColorTypeParser();
        
        Assert.Equal(new Type[]{typeof(TextColor)}, colorParser.SupportedTypes);
    }

    [Theory]
    [InlineData(new byte[]{1,2,3}, "123")]
    [InlineData(null, "")]
    public void RawString_Outputs_Correct_Config_Format(byte[]? color, string? expected)
    {
        TextColor textColor = null;

        if (color != null)
        {
            textColor = new TextColor(color[0], color[1], color[2]);
        }
        
        var colorParser = new TextColorTypeParser();

        var value = colorParser.ToRawString(textColor);
        
        Assert.Equal(expected, value);
    }
}
