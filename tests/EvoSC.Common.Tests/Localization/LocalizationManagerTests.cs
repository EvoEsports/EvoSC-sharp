using System.Globalization;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace EvoSC.Common.Tests.Localization;

public class LocalizationManagerTests
{
    private readonly ILocalizationManager _manager;

    public LocalizationManagerTests()
    {
        _manager = new LocalizationManager(typeof(LocalizationManagerTests).Assembly,
            "EvoSC.Common.Tests.Localization.TestLocalization", NullLogger<LocalizationManager>.Instance);
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
    public void Uses_Key_As_Value_When_Locale_Name_Was_Not_Found()
    {
        Assert.Equal("DoesNotExit", _manager.GetString(CultureInfo.InvariantCulture, "DoesNotExit"));
    }

    [Fact]
    public void Logs_A_Warning_When_Locale_Name_Was_Not_Found()
    {
        var logger = new Mock<ILogger<LocalizationManager>>();
        var manager = new LocalizationManager(typeof(LocalizationManagerTests).Assembly,
            "EvoSC.Common.Tests.Localization.TestLocalization", logger.Object);

        manager.GetString(CultureInfo.InvariantCulture, "DoesNotExit");

        logger.Verify(l => l.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Once);
    }
}
