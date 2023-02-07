using System.IO;
using EvoSC.Common.Util;
using EvoSC.Common.Util.MatchSettings.Builders;
using Xunit;

namespace EvoSC.Common.Tests.Util.MatchSettings;

public class MatchSettingsXmlSerializerTests
{
    [Fact]
    public void Generate_Default_MatchSettings_With_Mode()
    {
        var expected = File.ReadAllText("TestFiles/Util/MatchSettings/expected_default_matchsettings_with_mode.xml");
        var ms = new MatchSettingsBuilder()
            .WithMode("MyMode")
            .Build();

        var xml = ms.ToXmlDocument().GetFullXmlString();
        
        Assert.Equal(expected, xml);
    }

    [Fact]
    public void Generate_MatchSettings_With_ScriptSettings()
    {
        var expected = File.ReadAllText("TestFiles/Util/MatchSettings/expected_matchsettings_with_script_settings.xml");
        var ms = new MatchSettingsBuilder()
            .WithMode("MyMode")
            .WithModeSettings(s => s["S_MySetting"] = 1337)
            .Build();
        
        var xml = ms.ToXmlDocument().GetFullXmlString();
        
        Assert.Equal(expected, xml);
    }

    [Fact]
    public void Generate_MatchSettings_With_Maps()
    {
        var expected = File.ReadAllText("TestFiles/Util/MatchSettings/expected_matchsettings_with_maps.xml");
        var ms = new MatchSettingsBuilder()
            .WithMode("MyMode")
            .AddMap("MyMap.Map.Gbx", "my-ident")
            .Build();
        
        var xml = ms.ToXmlDocument().GetFullXmlString();
        
        Assert.Equal(expected, xml);
    }
}
