using EvoSC.Common.Util;
using EvoSC.Common.Util.MatchSettings.Builders;
using Xunit;

namespace EvoSC.Common.Tests.Util.MatchSettings;

public class MatchSettingsXmlSerializerTests
{
    [Fact]
    public void Generate_Default_MatchSettings_With_Mode()
    {
        var ms = new MatchSettingsBuilder()
            .WithMode("MyMode")
            .Build();

        var xml = ms.ToXmlDocument().GetFullXmlString();
        
        Assert.Equal("""
            <?xml version="1.0" encoding="utf-8"?>
            <playlist>
              <gameinfos>
                <game_mode>0</game_mode>
                <chat_time>10000</chat_time>
                <finishtimeout>1</finishtimeout>
                <allwarmupduration>false</allwarmupduration>
                <disablerespawn>false</disablerespawn>
                <forceshowallopponents>false</forceshowallopponents>
                <script_name>MyMode</script_name>
              </gameinfos>
              <filter>
                <is_lan>true</is_lan>
                <is_internet>true</is_internet>
                <is_solo>false</is_solo>
                <is_hotseat>false</is_hotseat>
                <sort_index>1000</sort_index>
                <random_map_order>false</random_map_order>
              </filter>
              <start_index>0</start_index>
            </playlist>
            """, xml);
    }

    [Fact]
    public void Generate_MatchSettings_With_ScriptSettings()
    {
        var ms = new MatchSettingsBuilder()
            .WithMode("MyMode")
            .WithModeSettings(s => s["S_MySetting"] = 1337)
            .Build();
        
        var xml = ms.ToXmlDocument().GetFullXmlString();
        
        Assert.Equal("""
            <?xml version="1.0" encoding="utf-8"?>
            <playlist>
              <gameinfos>
                <game_mode>0</game_mode>
                <chat_time>10000</chat_time>
                <finishtimeout>1</finishtimeout>
                <allwarmupduration>false</allwarmupduration>
                <disablerespawn>false</disablerespawn>
                <forceshowallopponents>false</forceshowallopponents>
                <script_name>MyMode</script_name>
              </gameinfos>
              <filter>
                <is_lan>true</is_lan>
                <is_internet>true</is_internet>
                <is_solo>false</is_solo>
                <is_hotseat>false</is_hotseat>
                <sort_index>1000</sort_index>
                <random_map_order>false</random_map_order>
              </filter>
              <script_settings>
                <setting name="S_MySetting" value="1337" type="integer" />
              </script_settings>
              <start_index>0</start_index>
            </playlist>
            """, xml);
    }

    [Fact]
    public void Generate_MatchSettings_With_Maps()
    {
        var ms = new MatchSettingsBuilder()
            .WithMode("MyMode")
            .AddMap("MyMap.Map.Gbx", "my-ident")
            .Build();
        
        var xml = ms.ToXmlDocument().GetFullXmlString();
        
        Assert.Equal("""
            <?xml version="1.0" encoding="utf-8"?>
            <playlist>
              <gameinfos>
                <game_mode>0</game_mode>
                <chat_time>10000</chat_time>
                <finishtimeout>1</finishtimeout>
                <allwarmupduration>false</allwarmupduration>
                <disablerespawn>false</disablerespawn>
                <forceshowallopponents>false</forceshowallopponents>
                <script_name>MyMode</script_name>
              </gameinfos>
              <filter>
                <is_lan>true</is_lan>
                <is_internet>true</is_internet>
                <is_solo>false</is_solo>
                <is_hotseat>false</is_hotseat>
                <sort_index>1000</sort_index>
                <random_map_order>false</random_map_order>
              </filter>
              <start_index>0</start_index>
              <map>
                <file>MyMap.Map.Gbx</file>
                <ident>my-ident</ident>
              </map>
            </playlist>
            """, xml);
    }
}
