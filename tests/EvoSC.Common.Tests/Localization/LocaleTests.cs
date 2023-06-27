using System.Collections;
using System.Linq;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Localization;
using EvoSC.Common.Models.Audit;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Util.Auditing;
using Moq;
using Xunit;

namespace EvoSC.Common.Tests.Localization;

public class LocaleTests
{
    private readonly ILocalizationManager _manager;
    private readonly Mock<IContextService> _contextService;
    private readonly Mock<IEvoScBaseConfig> _config;

    public LocaleTests()
    {
        _manager = new LocalizationManager(typeof(LocalizationManagerTests).Assembly,
            "EvoSC.Common.Tests.Localization.TestLocalization");

        var localeConfigMock = new Mock<ILocaleConfig>();
        localeConfigMock.Setup(lc => lc.DefaultLanguage)
            .Returns("en");

        var auditService = new Mock<IAuditService>();
        auditService.Setup(a => a.LogAsync(
            It.IsAny<string>(),
            It.IsAny<AuditEventStatus>(),
            It.IsAny<IPlayer>(),
            It.IsAny<string>(),
            It.IsAny<object>())
        );
        var baseContext = new GenericControllerContext {
            Controller = null, 
            AuditEvent = new AuditEventBuilder(auditService.Object)
        };
        
        _contextService = new Mock<IContextService>();
        _contextService.Setup(c => c.GetContext())
            .Returns(new PlayerInteractionContext(new OnlinePlayer
            {
                Id = 0,
                AccountId = null,
                NickName = null,
                UbisoftName = null,
                Zone = null,
                Settings = new DbPlayerSettings
                {
                    PlayerId = 0,
                    DisplayLanguage = "nb-no"
                },
                State = PlayerState.Spectating,
                Flags = null
            }, baseContext) {Controller = null, AuditEvent = null});
        
        _config = new Mock<IEvoScBaseConfig>();
        _config.Setup(c => c.Locale)
            .Returns(localeConfigMock.Object);
    }

    [Fact]
    public void Get_Locale_From_Indexer()
    {
        var locale = new LocaleResource(_manager, _contextService.Object, _config.Object);

        var result = locale["TestKey"];
        
        Assert.Equal("This is a sentence.", result);
    }

    [Fact]
    public void Returns_Player_Defined_Locale()
    {
        var locale = new LocaleResource(_manager, _contextService.Object, _config.Object);

        var result = locale.PlayerLanguage["TestKey"];
        
        Assert.Equal("Dette er en setning.", result);
    }

    [Theory]
    [InlineData("[TestKey]", "This is a sentence.")]
    [InlineData("[TestKey][TestKey]", "This is a sentence.This is a sentence.")]
    [InlineData("A random [TestKey] string [TestKey] with [TestKey] Locales in [TestKey] between.", "A random This is a sentence. string This is a sentence. with This is a sentence. Locales in This is a sentence. between.")]
    public void Replaces_Locales_In_Arbitrary_Strings(string toTranslate, string expected)
    {
        var locale = new LocaleResource(_manager, _contextService.Object, _config.Object);

        var result = locale.Translate(toTranslate);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Dynamic_Accessor_Returns_Locale()
    {
        dynamic locale = new LocaleResource(_manager, _contextService.Object, _config.Object);

        var result = locale.TestKey;
        
        Assert.Equal("This is a sentence.", result);
    }
    
    [Fact]
    public void Dynamic_Accessor_Returns_Locale_With_Args()
    {
        dynamic locale = new LocaleResource(_manager, _contextService.Object, _config.Object);

        var result = locale.TestKeyWithArgs("My Argument");
        
        Assert.Equal("This is the argument: My Argument", result);
    }

    [Fact]
    public void Returns_Resource_Set()
    {
        var locale = new LocaleResource(_manager, _contextService.Object, _config.Object);

        var resources = locale
            .GetResourceSet()?
            .Cast<DictionaryEntry>()
            .ToArray();
        
        Assert.NotNull(resources);
        Assert.Equal("TestKey", resources[0].Key);
        Assert.Equal("This is a sentence.", resources[0].Value);
        Assert.Equal("TestKeyWithArgs", resources[1].Key);
        Assert.Equal("This is the argument: {0}", resources[1].Value);
    }
    
    [Fact]
    public void Returns_Resource_Set_Of_Player_Language()
    {
        var locale = new LocaleResource(_manager, _contextService.Object, _config.Object);

        var resources = locale
            .PlayerLanguage
            .GetResourceSet()?
            .Cast<DictionaryEntry>()
            .ToArray();
        
        Assert.NotNull(resources);
        Assert.Equal("TestKey", resources[0].Key);
        Assert.Equal("Dette er en setning.", resources[0].Value);
        Assert.Equal("TestKeyWithArgs", resources[1].Key);
        Assert.Equal("Dette er argumentet: {0}", resources[1].Value);
    }
}
