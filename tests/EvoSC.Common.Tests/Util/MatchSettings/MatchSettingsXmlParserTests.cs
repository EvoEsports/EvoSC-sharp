using System;
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
        var xml = """
            <?xml version="1.0" encoding="utf-8"?>
            <playlist>
              <gameinfos>
                <game_mode>123</game_mode>
                <chat_time>73465</chat_time>
                <finishtimeout>46571</finishtimeout>
                <allwarmupduration>true</allwarmupduration>
                <disablerespawn>true</disablerespawn>
                <forceshowallopponents>true</forceshowallopponents>
                <script_name>MyMode</script_name>
              </gameinfos>
              <filter>
                <is_lan>false</is_lan>
                <is_internet>false</is_internet>
                <is_solo>true</is_solo>
                <is_hotseat>true</is_hotseat>
                <sort_index>3425</sort_index>
                <random_map_order>true</random_map_order>
              </filter>
              <script_settings>
                <setting name="S_MySetting" value="1337" type="integer" />
              </script_settings>
              <start_index>24627</start_index>
              <map>
                <file>MyMap.Map.Gbx</file>
                <ident>my-ident</ident>
              </map>
            </playlist>
            """;

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
        var xml = """
            <?xml version="1.0" encoding="utf-8"?>
            <playlist>
              <gameinfos>
                <game_mode>123</game_mode>
                <chat_time>73465</chat_time>
                <finishtimeout>46571</finishtimeout>
                <allwarmupduration>true</allwarmupduration>
                <disablerespawn>true</disablerespawn>
                <forceshowallopponents>true</forceshowallopponents>
                <script_name>MyMode</script_name>
              </gameinfos>
              <filter>
                <is_lan>false</is_lan>
                <is_internet>false</is_internet>
                <is_solo>true</is_solo>
                <is_hotseat>true</is_hotseat>
                <sort_index>3425</sort_index>
                <random_map_order>true</random_map_order>
              </filter>
              <script_settings>
                <setting value="1337" type="integer" />
              </script_settings>
              <start_index>24627</start_index>
            </playlist>
            """;

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await MatchSettingsXmlParser.ParseAsync(xml);
        });
    }
    
    [Fact]
    public async Task Use_Default_Value_For_Element()
    {
        var xml = """
            <?xml version="1.0" encoding="utf-8"?>
            <playlist>
              <gameinfos>
                <game_mode></game_mode>
                <chat_time></chat_time>
                <finishtimeout>46571</finishtimeout>
                <allwarmupduration>true</allwarmupduration>
                <disablerespawn>true</disablerespawn>
                <forceshowallopponents>true</forceshowallopponents>
                <script_name>Trackmania/TM_TimeAttack_Online.Script.txt</script_name>
              </gameinfos>
              <filter>
                <is_lan>false</is_lan>
                <is_internet>false</is_internet>
                <is_solo>true</is_solo>
                <is_hotseat>true</is_hotseat>
                <sort_index>3425</sort_index>
                <random_map_order>true</random_map_order>
              </filter>
              <start_index>24627</start_index>
            </playlist>
            """;

        var ms = await MatchSettingsXmlParser.ParseAsync(xml);
        
        Assert.NotNull(ms.GameInfos);
        Assert.Equal(10000, ms.GameInfos.ChatTime);
    }
}
