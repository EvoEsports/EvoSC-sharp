using System.Collections.Generic;
using System.Globalization;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Localization;
using Xunit;

namespace EvoSC.Common.Tests.Localization;

public class LocalizationManagerTests
{
    private readonly ILocalizationManager _manager;

    public LocalizationManagerTests()
    {
        _manager = new LocalizationManager(typeof(LocalizationManagerTests).Assembly,
            "EvoSC.Common.Tests.Localization.TestLocalization");
    }

    [Theory]
    [InlineData("en", "This is a sentence.")]
    [InlineData("nb-no", "Dette er en setning.")]
    public void Basic_Local_Retrieved_In_Different_Cultures(string cultureName, string expected)
    {
        var actual = _manager.GetString(new CultureInfo(cultureName), "TestKey");
        
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Throws_If_Locale_Name_Was_Not_Found()
    {
        Assert.Throws<KeyNotFoundException>(() => _manager.GetString(CultureInfo.InvariantCulture, "DoesNotExit"));
    }
}
