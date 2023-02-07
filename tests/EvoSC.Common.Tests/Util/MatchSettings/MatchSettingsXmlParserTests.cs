using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EvoSC.Common.Util.MatchSettings;
using Xunit;

namespace EvoSC.Common.Tests.Util.MatchSettings;

public class MatchSettingsXmlParserTests
{
    [Fact]
    public async Task Parses_MatchSettings_With_All_Sections()
    {
        var xml = File.ReadAllText("TestFiles/Util/MatchSettings/matchsettings_with_all_sections.xml");

        var ms = await MatchSettingsXmlParser.ParseAsync(xml);
        
        Assert.NotNull(ms);
        Assert.NotNull(ms.GameInfos);
        Assert.NotNull(ms.Filter);
        Assert.NotNull(ms.ModeScriptSettings);
        Assert.NotNull(ms.Maps);
        
        Assert.Equal(24627, ms.StartIndex);
        
        Assert.Equal(123, ms.GameInfos.GameMode);
        Assert.Equal(73465, ms.GameInfos.ChatTime);
        Assert.Equal(46571, ms.GameInfos.FinishTimeout);
        Assert.True(ms.GameInfos.AllWarmupDuration);
        Assert.True(ms.GameInfos.DisableRespawn);
        Assert.True(ms.GameInfos.ForceShowAllOpponents);
        Assert.Equal("MyMode", ms.GameInfos.ScriptName);
        
        Assert.False(ms.Filter.IsLan);
        Assert.False(ms.Filter.IsInternet);
        Assert.True(ms.Filter.IsSolo);
        Assert.True(ms.Filter.IsHotseat);
        Assert.True(ms.Filter.RandomMapOrder);
        Assert.Equal(3425, ms.Filter.SortIndex);
        
        Assert.NotNull(ms.ModeScriptSettings["S_MySetting"]);
        Assert.Equal(typeof(int), ms.ModeScriptSettings["S_MySetting"].Type);
        Assert.Equal(1337, ms.ModeScriptSettings["S_MySetting"].Value);
        
        Assert.NotNull(ms.Maps.FirstOrDefault());
        Assert.Equal("MyMap.Map.Gbx", ms.Maps.FirstOrDefault()?.FilePath);
        Assert.Equal("my-ident", ms.Maps.FirstOrDefault()?.Uid);
    }

    [Fact]
    public async Task Throw_If_ModeScript_Setting_Misses_Attribute()
    {
        var xml = File.ReadAllText("TestFiles/Util/MatchSettings/matchsettings_with_missing_attribute.xml");

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await MatchSettingsXmlParser.ParseAsync(xml);
        });
    }
    
    [Fact]
    public async Task Use_Default_Value_For_Element()
    {
        var xml = File.ReadAllText("TestFiles/Util/MatchSettings/matchsettings_for_default_values.xml");

        var ms = await MatchSettingsXmlParser.ParseAsync(xml);
        
        Assert.NotNull(ms.GameInfos);
        Assert.Equal(10000, ms.GameInfos.ChatTime);
    }
}
