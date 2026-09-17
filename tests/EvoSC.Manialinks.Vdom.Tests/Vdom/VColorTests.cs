using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Tests.Vdom;

public class VColorTests
{
    [Fact]
    public void Parses_6_digit_hex()
    {
        var color = VColor.FromHex("FF8000");

        Assert.Equal(1.0, color.R, 3);
        Assert.Equal(128.0 / 255.0, color.G, 3);
        Assert.Equal(0.0, color.B, 3);
    }

    [Fact]
    public void Parses_3_digit_shorthand_hex()
    {
        var color = VColor.FromHex("0AF");

        Assert.Equal(0.0, color.R, 3);
        Assert.Equal(10.0 / 15.0, color.G, 3);
        Assert.Equal(1.0, color.B, 3);
    }

    [Fact]
    public void Strips_a_leading_hash()
    {
        Assert.Equal(VColor.FromHex("FFF"), VColor.FromHex("#FFF"));
    }

    [Fact]
    public void ToHex_round_trips_through_FromHex()
    {
        Assert.Equal("FF8000", VColor.FromHex("FF8000").ToHex());
    }

    [Theory]
    [InlineData("")]
    [InlineData("FF")]
    [InlineData("FFFF")]
    [InlineData("FFFFFFF")]
    public void Rejects_the_wrong_number_of_digits(string hex)
    {
        Assert.Throws<ArgumentException>(() => VColor.FromHex(hex));
    }
}
