using System.Reflection;
using EvoSC.Common.Util.TextFormatting;
using Xunit;

namespace EvoSC.Common.Tests.Util;

public class ColorUtilsTests
{
    [Theory]
    [InlineData("ffffff", 100, "ffffff")]
    [InlineData("ffffff", 0, "000000")]
    [InlineData("ff0058", 0, "000000")]
    [InlineData("ff0058", 10, "330012")]
    [InlineData("ff0058", 20, "660023")]
    [InlineData("ff0058", 30, "990035")]
    [InlineData("ff0058", 40, "cc0046")]
    [InlineData("ff0058", 50, "ff0058")]
    [InlineData("ff0058", 60, "ff3379")]
    [InlineData("ff0058", 70, "ff669b")]
    [InlineData("ff0058", 80, "ff99bc")]
    [InlineData("ff0058", 90, "ffccde")]
    [InlineData("ff0058", 100, "ffffff")]
    public void Lightness_Set_For_HexColor(string hexIn, float lightness, string hexOut)
    {
        hexOut = hexOut.ToUpper();
        var newColor = ColorUtils.SetLightness(hexIn, lightness);
        
        Assert.Equal(hexOut, newColor);
    }
    
    [Theory]
    [InlineData("ffffff", 100, "ffffff")]
    [InlineData("ffffff", 0, "000000")]
    [InlineData("f05", 0, "000000")]
    [InlineData("f05", 10, "330011")]
    [InlineData("f05", 20, "660022")]
    [InlineData("f05", 30, "990033")]
    [InlineData("f05", 40, "cc0044")]
    [InlineData("f05", 50, "ff0055")]
    [InlineData("f05", 60, "ff3377")]
    [InlineData("f05", 70, "ff6699")]
    [InlineData("f05", 80, "ff99bb")]
    [InlineData("f05", 90, "ffccdd")]
    [InlineData("f05", 100, "ffffff")]
    public void Lightness_Set_For_TextColor(string hexIn, float lightness, string hexOut)
    {
        hexOut = hexOut.ToUpper();
        var textColor = new TextColor(hexIn);
        var newColor = ColorUtils.SetLightness(textColor, lightness);
        
        Assert.Equal(hexOut, newColor);
    }

    [Fact]
    public void Hex_Color_Lightened_By_10_Percent()
    {
        var newColor = ColorUtils.Lighten("ff0058");
        
        Assert.Equal("FF3379", newColor);
    }
    
    [Fact]
    public void Hex_Color_Darkened_By_10_Percent()
    {
        var newColor = ColorUtils.Darken("ff0058");
        
        Assert.Equal("CC0046", newColor);
    }
}
