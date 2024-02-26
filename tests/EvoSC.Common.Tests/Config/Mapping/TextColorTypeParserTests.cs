using System;
using EvoSC.Common.Config.Mapping;
using EvoSC.Common.Config.Mapping.Toml;
using EvoSC.Common.Util.TextFormatting;
using Tomlet.Exceptions;
using Tomlet.Models;
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

        var parsed = colorParser.TryParse(value, null, out var result);

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

    [Theory]
    [InlineData("123", new byte[]{1, 2, 3})]
    [InlineData("000", new byte[]{0, 0, 0})]
    [InlineData("fff", new byte[]{0xf, 0xf, 0xf})]
    public void Mapper_Serializes_Color_Correctly(string expected, byte[] color)
    {
        var mapper = new TextColorTomlMapper();

        var value = mapper.Serialize(new TextColor(color[0], color[1], color[2]));

        Assert.Equal(expected, value.StringValue);
    }

    [Theory]
    [InlineData("123", new byte[]{1, 2, 3})]
    [InlineData("000", new byte[]{0, 0, 0})]
    [InlineData("fff", new byte[]{0xf, 0xf, 0xf})]
    public void Mapper_Deserializes_Color_Correctly(string value, byte[] color)
    {
        var mapper = new TextColorTomlMapper();
        var tomlValue = new TomlString(value);
        var r = Convert.ToHexString(color, 0, 1)[1..];
        var g = Convert.ToHexString(color, 1, 1)[1..];
        var b = Convert.ToHexString(color, 2, 1)[1..];

        var textColor = mapper.Deserialize(tomlValue);

        var expected = $"${r}{g}{b}".ToLower();
        
        Assert.Equal(expected, textColor.ToString());
    }

    [Fact]
    public void Mapper_Deserializer_Throws_On_Wrong_Toml_Value_Type()
    {
        var mapper = new TextColorTomlMapper();
        var tomlValue = new TomlLong(123);

        Assert.Throws<TomlTypeMismatchException>(() => mapper.Deserialize(tomlValue));
    }
}
